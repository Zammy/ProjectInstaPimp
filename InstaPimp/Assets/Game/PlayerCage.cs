using UnityEngine;
using System.Collections;

public class PlayerCage : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo;
        }
        set
        {
            playerInfo = value;
            SetPlayerInfo(value);
        }
    }

    void SetPlayerInfo(PlayerInfo playerInfo)
    {
        var playerGo = (GameObject) Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        playerGo.transform.parent = this.transform;
        Player player = playerGo.GetComponent<Player>();
        player.PlayerInfo = playerInfo;
    }
}
