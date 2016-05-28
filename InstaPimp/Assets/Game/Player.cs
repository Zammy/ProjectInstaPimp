using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float JumpSpeed = 12f;
    public float JumpTime = .4f;
    public float MoveSpeed = 8f;

    public CollisionChecker TopChecker;
    public CollisionChecker BottomChecker;
    public CollisionChecker LeftChecker;
    public CollisionChecker RightChecker;

    PlayerActions playerActions;
    new Rigidbody rigidbody;
    public bool isGrounded = false;
    float jumpUntil = float.MinValue;
    float move;

    //const float OUTSET = 0.001f;

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
    }

    void FixedUpdate()
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
            if (move > 0 && !RightChecker.IsCollidingWith("Wall"))
            {
                x = MoveSpeed;
            }

            if (move < 0 && !LeftChecker.IsCollidingWith("Wall"))
            {
                x = -MoveSpeed;
            }
        }

        rigidbody.velocity = new Vector3(x, y, 0);

        //float diff = Mathf.Abs(transform.position.y - Mathf.RoundToInt(transform.position.y));
        //if (isGrounded && diff < OUTSET)
        //{
        //    Debug.Log("Outset diff:" + diff);
        //    transform.position = new Vector3(transform.position.x, transform.position.y + OUTSET, 0);
        //}
    }

    void OnGUI()
    {
        //var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //var screenHeight = Screen.height;

        //GUI.Label(new Rect(screenPos.x + 15, screenHeight - screenPos.y - 10, 100, 50), RightChecker.IsCollidingWith("Wall").ToString() );
        //GUI.Label(new Rect(screenPos.x - 50, screenHeight - screenPos.y - 10, 100, 50), LeftChecker.IsCollidingWith("Wall").ToString());
    }
}
