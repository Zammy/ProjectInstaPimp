using UnityEngine;
using System.Collections;

public class RailShot : MonoBehaviour
{ 
    public LineRenderer LineRenderer;

    public void Shoot(Transform nozzle)
    {
        RaycastHit hit;
        if (!Physics.Raycast(nozzle.position, nozzle.up, out hit))
        {
            Debug.LogError("Raycasted and hit nothing!");
        }

        LineRenderer.SetPosition(0, nozzle.position);
        LineRenderer.SetPosition(1, hit.point);
    }
}
