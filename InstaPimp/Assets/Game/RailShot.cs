using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RailShot : MonoBehaviour
{
    public LineRenderer LineRenderer;

    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
            LineRenderer.material = player.PlayerInfo.Material;
        }
    }

    public void Shoot(Transform nozzle)
    {
        LineRenderer.SetWidth(0.1f, 0.1f);

        RaycastHit hit;
        if (!Physics.Raycast(nozzle.position,
             nozzle.up, 
             out hit,  
             float.MaxValue, 
             LayerMask.GetMask(new string[] { "Wall", "Player"})))
        {
            Debug.LogError("Raycasted and hit nothing!");
            return;
        }

        LineRenderer.SetPosition(0, nozzle.position);
        LineRenderer.SetPosition(1, hit.point);

        if (hit.collider.tag == "Player")
        {
            var player = hit.collider.transform.parent.GetComponent<Player>();
            if (!player.IsDead)
            {
                StartCoroutine( Kill( player)  );

            }
        }

        StartCoroutine(Die());
    }

    private IEnumerator Kill(Player player)
    {
        yield return null;
        player.IsDead = true;
        GameController.Instance.PlayerKilledPlayer(this.Player, player);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);
        yield return this.LineRenderer.material.DOFade(0, 0.4f).WaitForCompletion();

        Destroy(this.gameObject);
    }
}
