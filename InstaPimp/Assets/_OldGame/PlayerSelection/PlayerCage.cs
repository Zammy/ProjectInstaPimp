using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCage : MonoBehaviour
{
    public GameObject ReadyIndicator;
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

    public bool IsReady
    {
        get
        {
            return ReadyIndicator.activeSelf;
        }
        set
        {
            ReadyIndicator.SetActive(value);
        }
    }

    void SetPlayerInfo(PlayerInfo playerInfo)
    {
        var playerGo = (GameObject) Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        playerGo.transform.parent = this.transform;
        Player player = playerGo.GetComponent<Player>();
        player.PlayerInfo = playerInfo;
    }

    void Update()
    {
        this.IsReady = playerInfo.Device.Action1.IsPressed;
    }
}
