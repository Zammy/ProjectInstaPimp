using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject PlayerCagePrefab;
    public Material[] PlayerColors;
    public float ReadyTime = 3f;

    List<PlayerCage> playerCages = new List<PlayerCage>();

    List<InputDevice> pendingDetachedDevices = new List<InputDevice>();
    float allDevicesReadyTime = float.MinValue;

    void Start()
    {
    }

    void OnEnable()
    {
        InputManager.OnDeviceAttached += OnDeviceAttached;
        InputManager.OnDeviceDetached += OnDeviceDetached;
    }

    void OnDisable()
    {
        InputManager.OnDeviceAttached -= OnDeviceAttached;
        InputManager.OnDeviceDetached -= OnDeviceDetached;
    }

    void FixedUpdate()
    {
        int activePlayers = playerCages.Count;

        InputDevice activeDevice = InputManager.ActiveDevice;

        if (activeDevice.Action1.WasPressed
            && FindPlayerCage(activeDevice) == null
            && activePlayers <= 4)
        {
            this.AddPlayer(activeDevice);
        }

        if (activeDevice.Action2.WasPressed
             && FindPlayerCage(activeDevice) != null)
        {
            this.RemovePlayer(activeDevice);
        }

        CheckIfShouldStartGame();
    }

    private void CheckIfShouldStartGame()
    {
        if (playerCages.Count <= 1)
            return;

        for (int i = 0; i < playerCages.Count; i++)
        {
            if (!playerCages[i].IsReady)
            {
                allDevicesReadyTime = float.MinValue;
                return;
            }
        }

        if (allDevicesReadyTime < 0)
        {
            allDevicesReadyTime = Time.time + ReadyTime;
        }
        else if (Time.time > allDevicesReadyTime)
        {  
            GameInfo.Players = this.playerCages.Select<PlayerCage, PlayerInfo>(pc => pc.PlayerInfo).ToList();
            SceneManager.LoadScene("Test");
        }
    }

    void OnDeviceAttached(InputDevice inputDevice)
    {
        Debug.LogFormat("{0} attached", inputDevice.Name);
        foreach (var device in pendingDetachedDevices)
        {
            if (device.Name == inputDevice.Name)
            {
                pendingDetachedDevices.Remove(device);
                var cage = FindPlayerCage(device);
                cage.PlayerInfo.Device = inputDevice;
                break;
            }
        }
    }

    void OnDeviceDetached(InputDevice inputDevice)
    {
        Debug.LogFormat("{0} detached", inputDevice.Name);
        pendingDetachedDevices.Add(inputDevice);

        StopCoroutine("CheckDevices");
        StartCoroutine("CheckDevices");
    }

    IEnumerator CheckDevices()
    {
        yield return null;
        foreach (var device in pendingDetachedDevices)
        {
            RemovePlayer(device);
        }
        pendingDetachedDevices.Clear();
    }

    private void AddPlayer(InputDevice activeDevice)
    {
        var playerActions = PlayerActions.CreateWithDefaultBindings();
        playerActions.Device = activeDevice;

        var playerCageGo = (GameObject)Instantiate(PlayerCagePrefab);
        PlayerCage playerCage = playerCageGo.GetComponent<PlayerCage>();
        playerCage.PlayerInfo = new PlayerInfo()
        {
            PlayerActions = playerActions,
            Device = activeDevice,
            Material = GetAvailableMaterial(),
        };
        this.playerCages.Add(playerCage);

        UpdateCagesPosition();
    }

    private void RemovePlayer(InputDevice activeDevice)
    {
        var playerCage = FindPlayerCage(activeDevice);
        if (playerCage != null)
        {
            playerCages.Remove(playerCage);
            Destroy(playerCage.gameObject);
            UpdateCagesPosition();
        }
    }

    private PlayerCage FindPlayerCage(InputDevice device)
    {
        foreach (var cage in playerCages)
        {
            if (cage.PlayerInfo.Device.Name == device.Name)
            {
                return cage;
            }
        }

        return null;
    }

    private void UpdateCagesPosition()
    {
        int playerCount = playerCages.Count;
        switch (playerCount)
        {
            case 1:
                {
                    playerCages[0].transform.position = Vector3.zero;
                    break;
                }
            case 2:
                {
                    playerCages[0].transform.position = new Vector3(-5, 0, 0);
                    playerCages[1].transform.position = new Vector3(5, 0, 0);
                    break;
                }
            case 3:
                {
                    playerCages[0].transform.position = new Vector3(-7, 0, 0);
                    playerCages[1].transform.position = Vector3.zero;
                    playerCages[0].transform.position = new Vector3(7, 0, 0);
                    break;
                }
            case 4:
                {
                    playerCages[0].transform.position = new Vector3(-12, 0, 0);
                    playerCages[1].transform.position = new Vector3(-4, 0, 0);
                    playerCages[2].transform.position = new Vector3(4, 0, 0);
                    playerCages[3].transform.position = new Vector3(12, 0, 0);
                    break;
                }
            default:
                break;
        }
    }

    private Material GetAvailableMaterial()
    {
        var playerColors = new List<Material>(PlayerColors);
        foreach (var cage in playerCages)
        {
            playerColors.Remove(cage.PlayerInfo.Material);
        }

        return playerColors[0];
    }
}
