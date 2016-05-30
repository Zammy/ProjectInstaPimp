using UnityEngine;
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

    PlayerActions playerActions;
    new Rigidbody rigidbody;
    public bool isGrounded = false;
    float jumpUntil = float.MinValue;
    float move;
    Vector2 aim;
    bool shoot;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

    void Update()
    {
        isGrounded = BottomChecker.IsCollidingWith("Wall");

        if (isGrounded && playerActions.Jump.WasPressed)
        {
            jumpUntil = Time.time + JumpTime;
        }
        move = playerActions.Move.Value;

        var newAim = new Vector2(playerActions.Aim.X, playerActions.Aim.Y);
        if (newAim.sqrMagnitude > 0.5f)
        {
            aim = newAim;
        }

        shoot = playerActions.Fire.WasPressed;
    }

    void FixedUpdate()
    {
        MovementUpdate();
        AimUpdate();
        
        if (shoot)
        {
            var projGo = (GameObject)Instantiate(ProjPrefab, Nozzle.position, Nozzle.rotation);
            RailShot railShot = projGo.GetComponent<RailShot>();
            railShot.Shoot(Nozzle);
        }
    }

    private void MovementUpdate()
    {
        float x = 0;
        float y = rigidbody.velocity.y;

        if (Time.time < jumpUntil)
        {
            if (TopChecker.IsCollidingWith("Wall"))
            {
                jumpUntil = float.MinValue;
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

    void AimUpdate()
    {
        Vector3 aimVec = new Vector3(aim.x, aim.y, 0).normalized;
        this.Aim.xLookAt(transform.position + aimVec);
    }

    void OnGUI()
    {
        //var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //var screenHeight = Screen.height;

        //GUI.Label(new Rect(screenPos.x + 15, screenHeight - screenPos.y - 10, 100, 50), RightChecker.IsCollidingWith("Wall").ToString() );
        //GUI.Label(new Rect(screenPos.x - 50, screenHeight - screenPos.y - 10, 100, 50), LeftChecker.IsCollidingWith("Wall").ToString());
    }
}
