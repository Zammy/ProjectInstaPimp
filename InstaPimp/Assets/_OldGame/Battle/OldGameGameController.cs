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
    BulletTime,
    PostPlay
}

public class OldGameGameController : SingletonBehavior<OldGameGameController>
{
    //UI
    public Text Announcer;
    public TimerDisplay TimerDisplay;
    public Flasher Flasher;
    public Transform ScoreBase;
    public GameObject ScoreSheetPrefab;
    public ScorePanel[] ScorePanels;
    //

    public GameObject PlayerPrefab;
    public Transform PlayersBase;
    public Transform RailShotsBase;

    public Material[] PlayerColors;

    public float FreezePauseTime = 1.5f;
    public float ShootDelay = 0.25f;
    public float ShootInterval = 2f;
    public float DelayTimeScale = 0.25f;


    public Transform[] TwoPlayerStartingPoints;
    public Transform[] ThreePlayerStartingPoints;
    public Transform[] FourPlayerStartingPoints;
    public Transform[] DeathmatchSpawnPoints;


    private List<Player> players;
    //private int[] playerScores;

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
//            Debug.LogFormat("Change state {0} >> {1} ", state, value);

            state = value;
            switch (state)
            {
                case PlayState.PrePlay:
                    StartCoroutine(GameStarted());

                    List<Transform> startingPoints = null;
                    switch (players.Count)
                    {
                        case 2:
                            {
                                startingPoints = new List<Transform>(TwoPlayerStartingPoints);
                                break;
                            }
                        case 3:
                            {
                                startingPoints = new List<Transform>(ThreePlayerStartingPoints);
                                break;
                            }
                        case 4:
                            {
                                startingPoints = new List<Transform>(FourPlayerStartingPoints);
                                break;
                            }
                        default:
                            break;
                    }

                    startingPoints.xShuffle<Transform>();

                    for (int i = 0; i < players.Count; i++)
                    {
                        RespawnPlayer(players[i], startingPoints[i].position);
                    }

                    foreach (Transform child in RailShotsBase)
                    {
                        Destroy(child.gameObject);
                    }

                    if (GameInfo.GameMode == GameMode.Deathmatch)
                    {
                        foreach (var scorePanel in ScorePanels)
                        {
                            scorePanel.Score = 0;
                        }
                    }

                    break;
                case PlayState.Play:
                    Time.timeScale = 1f;

                    nextStateChangeTime = Time.fixedTime + ShootInterval;
                    for (int i = 0; i < players.Count; i++)
                    {
                        var sphereTimer = players[i].GetComponent<TimerSphere>();
                        sphereTimer.ResetTimeLeft(ShootInterval);
                    }
                    break;
                case PlayState.BulletTime:
                    Flasher.Flash();

                    Time.timeScale = DelayTimeScale;

                    DOTween.Sequence()
                        .AppendInterval(ShootDelay)
                        .AppendCallback(() =>
                        {
                            foreach (var player in players)
                            {
                                player.Shoot();
                            }
                        });
                    
                    nextStateChangeTime = Time.fixedTime + ShootDelay + FreezePauseTime;
                    break;
                case PlayState.PostPlay:
                    StartCoroutine(ShowResult());
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayerKilledPlayer(Player fragger, Player fragged)
    {
        //Debug.LogFormat("{0} killed {1}", fragger, fragged);

        int playerIndex = players.IndexOf(fragger);
        ScorePanels[playerIndex].Score++;

        if (GameInfo.GameMode == GameMode.LastManStanding)
        {
            fragged.IsDead = true;

            int alivePlayers = 0;
            foreach (var player in players)
            {
                if (!player.IsDead)
                    alivePlayers++;
            }

            if (alivePlayers <= 1)
                this.State = PlayState.PostPlay;
        }
        else if (GameInfo.GameMode == GameMode.Deathmatch)
        {
            if (ScorePanels[playerIndex].Score >= GameInfo.DeathmatchFragGoal)
            {
                this.State = PlayState.PostPlay;
            }

            fragged.IsDead = true;

//            nextStateChangeTime += 1f;

            DOTween.Sequence()
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    var newSpawnPoint = DeathmatchSpawnPoints[Random.Range(0, DeathmatchSpawnPoints.Length)].position;
                    RespawnPlayer(fragged, newSpawnPoint);
                });
        }
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
            var playerGo = (GameObject) Instantiate(PlayerPrefab);
            playerGo.transform.parent = PlayersBase;
            var player = playerGo.GetComponent<Player>();
            player.PlayerInfo = playerInfo;
            players.Add(player);

            var scoreSheet = (GameObject)Instantiate(ScoreSheetPrefab);
            scoreSheet.transform.parent = ScoreBase;
            scoreSheet.transform.localScale = Vector3.one;

            ScorePanels[i].Color = playerInfo.Material.color;
        }
        for (int i = GameInfo.Players.Count; i < 4; i++)
        {
            ScorePanels[i].gameObject.SetActive(false);
        }
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
                this.State = PlayState.BulletTime;
            else if (State == PlayState.BulletTime)
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

    void RespawnPlayer(Player player, Vector3 pos)
    {
        player.IsDead = false;
        player.transform.position = pos;
    }

    IEnumerator GameStarted()
    {
        yield return StartCoroutine(DoCountdown());
        this.State = PlayState.Play;
    }

    IEnumerator DoCountdown()
    {
        Announcer.gameObject.SetActive(true);

        //for (int i = 3; i > 0; i--)
        //{
        //    Announcer.text = i.ToString();
        //    Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //    yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();
        //}

        Announcer.text = "GO!";
        Announcer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return Announcer.transform.DOScale(1f, 1f).WaitForCompletion();

        Announcer.gameObject.SetActive(false);
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);

        this.ScoreBase.gameObject.SetActive(true);

        for (int i = 0; i < players.Count; i++)
        {
            var playerColor = players[i].PlayerInfo.Material.color;
            int score = ScorePanels[i].Score;

            var scoreSheet = ScoreBase.GetChild(i).GetComponent<ScoreSheet>();
            scoreSheet.SetKills(playerColor, score);
        }

        yield return new WaitForSeconds(1.5f);

        this.ScoreBase.gameObject.SetActive(false);

        this.State = PlayState.PrePlay;
    }

}
