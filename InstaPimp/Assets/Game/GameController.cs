using DG.Tweening;
using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayState
{
    Selection,
    PrePlay,
    Play,
    PreResolve,
    Resolve,
    PostResolve
}

public class GameController : SingletonBehavior<GameController>
{
    public GameObject PlayerPrefab;
    public Transform PlayersBase;
    public Text Announcer;
    public Transform[] StartingPoints;
    public Transform RailShotsBase;

    private int lastFrame;
    private int frameCounter = 0;

    private List<Player> players;



    private PlayState state;
    public PlayState State
    {
        get
        {
            return state;
        }
        private set
        {
            Debug.LogFormat("Change state {0} >> {1} ", state, value);

            state = value;
            switch (state)
            {
                case PlayState.PrePlay:
                    StartCoroutine(GameStarted());

                    var shotFrames = GameInfo.ShootFrames;

                    for (int i = 0; i < players.Count; i++)
                    {
                        var player = players[i];
                        player.SetShootFrames(shotFrames);
                        player.ResetInputIndex();
                    }

                    lastFrame = shotFrames[shotFrames.Count - 1];
                    break;
                case PlayState.Play:
                    frameCounter = 0;
                    break;
                case PlayState.PreResolve:
                    StartCoroutine(DoPreResolve());

                    for (int i = 0; i < players.Count; i++)
                    {
                        var player = players[i];
                        player.transform.position = StartingPoints[i].position;
                        player.ResetInputIndex();
                    }

                    frameCounter = 0;
                    break;
                case PlayState.Resolve:
                    break;
                case PlayState.PostResolve:
                    //TODO: add showing of result
                    //TODO: add delay before destroying rails
                    foreach (Transform child in RailShotsBase)
                    {
                        Destroy(child.gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        Debug.Log(InputManager.ActiveDevice);

        if (GameInfo.Players == null)
        {
            GameInfo.Players = new List<PlayerInfo>()
            {
                new PlayerInfo()
                {
                    PlayerActions = PlayerActions.CreateWithDefaultBindings(),
                    Device = InputManager.Devices[0]
                }
            };
        }
#endif
        players = new List<Player>();

        var shotFrames = GameInfo.ShootFrames;

        for (int i = 0; i < GameInfo.Players.Count; i++)
        {
            var playerInfo = GameInfo.Players[i];
            var playerGo = (GameObject) Instantiate(PlayerPrefab, StartingPoints[i].position, Quaternion.identity);
            playerGo.transform.parent = PlayersBase;
            var player = playerGo.GetComponent<Player>();
            player.PlayerInfo = playerInfo;
            players.Add(player);
        }

        lastFrame = shotFrames[shotFrames.Count - 1];
    }


    void Start()
    {
        this.State = PlayState.PrePlay;
    }

    void FixedUpdate()
    {
        if (State == PlayState.PrePlay
           || State == PlayState.PreResolve
           || State == PlayState.PostResolve)
        {
            return;
        }

        //Debug.LogFormat("[{0}][{1:f}] frameCounter {2} ", name, Time.fixedTime, frameCounter);

        if (frameCounter++ > lastFrame)
        {
            if (State == PlayState.Play)
                State = PlayState.PreResolve;
            else if (State == PlayState.Resolve)
                State = PlayState.PostResolve;
        }
    }

    IEnumerator GameStarted()
    {
        yield return StartCoroutine(DoCountdown());
        this.State = PlayState.Play;
    }

    IEnumerator DoCountdown()
    {
        Announcer.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            Announcer.text = i.ToString();
            Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();
        }

        Announcer.text = "GO!";
        Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();

        Announcer.gameObject.SetActive(false);
    }

    IEnumerator DoPreResolve()
    {
        Announcer.gameObject.SetActive(true);

        Announcer.text = "RESOLVE";
        Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();

        Announcer.gameObject.SetActive(false);

        State = PlayState.Resolve;
    }
}
