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
    Shoot,
    PostPlay
}

public class GameController : SingletonBehavior<GameController>
{
    public GameObject PlayerPrefab;
    public Transform PlayersBase;
    public Text Announcer;
    public Transform[] StartingPoints;
    public Transform RailShotsBase;

    public Transform ScoreBase;
    public GameObject ScoreSheetPrefab;

    public Material[] PlayerColors;

    public float ShootPauseTime = 1.5f;
    public float ShootInterval = 2f;

    public TimerDisplay TimerDisplay;

    private List<Player> players;
    private int[] playerScores;

    float nextStateChangeTime = float.MinValue;
    

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

                    for (int i = 0; i < players.Count; i++)
                    {
                        var player = players[i];
                        player.IsDead = false;
                        player.transform.position = StartingPoints[i].position;
                    }

                    foreach (Transform child in RailShotsBase)
                    {
                        Destroy(child.gameObject);
                    }

                    break;
                case PlayState.Play:
                    nextStateChangeTime = Time.fixedTime + ShootInterval;
                    for (int i = 0; i < players.Count; i++)
                    {
                        var sphereTimer = players[i].GetComponent<TimerSphere>();
                        sphereTimer.ResetTimeLeft(ShootInterval);
                    }
                    break;
                case PlayState.Shoot:
                    foreach (var player in players)
                    {
                        player.Shoot();
                    }

                    nextStateChangeTime = Time.fixedTime + ShootPauseTime;
                    break;
                case PlayState.PostPlay:
                    StartCoroutine(ShowResult());
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayerKilledPlayer(Player player1, Player player2)
    {
        Debug.LogFormat("{0} killed {1}", player1, player2);

        int playerIndex = players.IndexOf(player1);
        playerScores[playerIndex]++;

        int alivePlayers = 0;
        foreach (var player in players)
        {
            if (!player.IsDead)
                alivePlayers++;
        }

        if (alivePlayers <= 1)
            this.State = PlayState.PostPlay;
    }

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if (GameInfo.Players == null)
        {
            GameInfo.Players = new List<PlayerInfo>();

            for (int i = 0; i < InputManager.Devices.Count; i++)
            {
                GameInfo.Players.Add(new PlayerInfo()
                {
                    PlayerActions = PlayerActions.CreateWithDefaultBindings(),
                    Device = InputManager.Devices[i],
                    Material = PlayerColors[i]
                });
            }
        }
#endif
        players = new List<Player>();

        for (int i = 0; i < GameInfo.Players.Count; i++)
        {
            var playerInfo = GameInfo.Players[i];
            var playerGo = (GameObject) Instantiate(PlayerPrefab, StartingPoints[i].position, Quaternion.identity);
            playerGo.transform.parent = PlayersBase;
            var player = playerGo.GetComponent<Player>();
            player.PlayerInfo = playerInfo;
            players.Add(player);

            var scoreSheet = (GameObject)Instantiate(ScoreSheetPrefab);
            scoreSheet.transform.parent = ScoreBase;
            scoreSheet.transform.localScale = Vector3.one;
        }

        playerScores = new int[players.Count];
    }

    void Start()
    {
        this.State = PlayState.PrePlay;
    }

    void FixedUpdate()
    {
        if (State == PlayState.PrePlay
           || State == PlayState.PostPlay)
        {
            return;
        }

        //Debug.LogFormat("[{0}][{1:f}] frameCounter {2} ", name, Time.fixedTime, frameCounter);


        if (nextStateChangeTime < Time.fixedTime)
        {
            if (State == PlayState.Play)
                this.State = PlayState.Shoot;
            else if (State == PlayState.Shoot)
                this.State = PlayState.Play;
        }

        float timeLeft = nextStateChangeTime - Time.fixedTime;
        TimerDisplay.DisplayTimeLeft(timeLeft);

        if (State != PlayState.Play)
            return;

        for (int i = 0; i < players.Count; i++)
        {
            var sphereTimer = players[i].GetComponent<TimerSphere>();
            sphereTimer.DisplayTimeLeft(timeLeft);
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

    //IEnumerator DoPreResolve()
    //{
    //    Announcer.gameObject.SetActive(true);

    //    Announcer.text = "RESOLVE";
    //    Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    //    yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();

    //    Announcer.gameObject.SetActive(false);

    //    State = PlayState.Resolve;
    //}

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);

        this.ScoreBase.gameObject.SetActive(true);

        for (int i = 0; i < players.Count; i++)
        {
            var playerColor = players[i].PlayerInfo.Material.color;
            int score = playerScores[i];

            var scoreSheet = ScoreBase.GetChild(i).GetComponent<ScoreSheet>();
            scoreSheet.SetKills(playerColor, score);
        }

        yield return new WaitForSeconds(5f);

        this.ScoreBase.gameObject.SetActive(false);

        this.State = PlayState.PrePlay;
    }

}
