using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Player : MonoBehaviour
{
    public float JumpSpeed = 12f;
    public float JumpTime = .4f;
    public float MoveSpeed = 8f;

    public Transform Aim;
    public GameObject ProjPrefab;
    public Transform Nozzle;

    public CollisionChecker TopChecker;
    public CollisionChecker BottomChecker;
    public CollisionChecker NozzleChecker;

    public MeshRenderer Body;
    public MeshRenderer Railgun;

    Color baseColor;

    bool isGrounded = false;
    float jumpTimer = float.MinValue;

    Transform railShotsBase;
    GameController gameController;

    PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo;
        }
        set
        {
            playerInfo = value;
            Body.material = playerInfo.Material;
            Railgun.material = playerInfo.Material;
            GetComponent<TimerSphere>().SphereMaterial = playerInfo.Material;

            baseColor = playerInfo.Material.color;
        }
    }

    private bool isDead = false;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            isDead = value;

            if (isDead)
            {
                Die();
            }
            else
            {
                Undie();
            }
        }
    }

    public void Shoot()
    {
        if (NozzleChecker.IsCollidingWith("Wall") )
            return;

        var projGo = (GameObject)Instantiate(ProjPrefab, Nozzle.position, Nozzle.rotation);
        projGo.transform.parent = railShotsBase;
        RailShot railShot = projGo.GetComponent<RailShot>();
        railShot.Player = this;
        railShot.Shoot(Nozzle);
    }

    void Awake()
    {
        var obj = GameObject.Find("RailShots");
        if (obj != null)
            railShotsBase = obj.transform;

        this.gameController = GameController.Instance;
    }

    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(AimUpdate());
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        PlayState state = PlayState.Selection;
        if (gameController != null)
        {
            state = gameController.State;
        }

        if (state == PlayState.PrePlay
               || state == PlayState.PostPlay)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (state == PlayState.PrePlay
             || state == PlayState.PostPlay)
        {
            return;
        }

        jumpTimer -= Time.fixedDeltaTime;

        isGrounded = BottomChecker.IsCollidingWith("Wall") || BottomChecker.IsCollidingWith("Player");

        if (isGrounded && playerInfo.PlayerActions.Jump.WasPressed)
        {
            jumpTimer = JumpTime;
        }

        var move = playerInfo.PlayerActions.Move.Value;
        MovementUpdate(move);
    }

    private void MovementUpdate(float move)
    {
        var rigidbody = GetComponent<Rigidbody>();
        float x = 0;
        float y = rigidbody.velocity.y;

        if (jumpTimer > 0)
        {
            if (TopChecker.IsCollidingWith("Wall"))
            {
                jumpTimer = float.MinValue;
            }
            else
            {
                y = JumpSpeed;
            }
        }
        else if (!isGrounded)
        {
            y = -JumpSpeed; //gravity
        }

        if (Mathf.Abs(move) > 0.15f)
        {
            if (move > 0)
            {
                x = MoveSpeed;
            }

            if (move < 0)
            {
                x = -MoveSpeed;
            }
        }

        rigidbody.velocity = new Vector3(x, y, 0);
    }

    IEnumerator AimUpdate()
    {
        while (true)
        {
            var aim = playerInfo.PlayerActions.Aim.Value;
            if (aim.sqrMagnitude > 0.5f)
            {
                Vector3 aimVec = new Vector3(aim.x, aim.y, 0).normalized;
                this.Aim.xLookAt(transform.position + aimVec);
            }
            yield return null;
        }
    }

    void Die()
    {
        this.Body.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine( Fade() );
    }

    void Undie()
    {
        this.Body.GetComponent<BoxCollider>().enabled = true;
        this.Body.material.color = baseColor;
        this.Railgun.material.color = baseColor;
    }

    const float fadePerSec = 6f;
    IEnumerator Fade()
    {
        float time = 0f;
        Color startColor = this.Body.material.color;
        Color color = startColor;
        do 
        {
            color = Color.Lerp(startColor, Color.white, time);
            time += fadePerSec * Time.deltaTime;

            Body.material.color = color;
            Railgun.material.color = color;
            yield return null;
        }
        while( time < 1f);
    }
    
    void OnGUI()
    {
        //var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //var screenHeight = Screen.height;

        //GUI.Label(new Rect(screenPos.x + 15, screenHeight - screenPos.y - 10, 100, 50), RightChecker.IsCollidingWith("Wall").ToString() );
        //GUI.Label(new Rect(screenPos.x - 50, screenHeight - screenPos.y - 10, 100, 50), LeftChecker.IsCollidingWith("Wall").ToString());
    }
}
