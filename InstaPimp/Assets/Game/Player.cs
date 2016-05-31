using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;

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

    public MeshRenderer Body;
    public MeshRenderer Railgun;

    bool isGrounded = false;
    float jumpUntil = float.MinValue;

    const int FRAME_BUFFER_SIZE = 512;
    InputSnapshot[] input = new InputSnapshot[512];
    int frameCounter = 0;

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

    public void SetShootFrames(List<int> shootFrames)
    {
        foreach (var shootFrame in shootFrames)
        {
            input[shootFrame].FireWasPressed = true;
        }
    }

    public void ResetInputIndex()
    {
        frameCounter = 0;
    }

    void Awake()
    {
        var obj = GameObject.Find("RailShots");
        if (obj != null)
            railShotsBase = obj.transform;


        this.gameController = GameController.Instance;
    }

    void FixedUpdate()
    {
        if (IsDead)
            return;

        PlayState state = PlayState.Selection;
        if (gameController != null)
        {
            state = GameController.Instance.State;
        }

        if (state == PlayState.PrePlay
               || state == PlayState.PreResolve
               || state == PlayState.PostResolve)
        {
            return;
        }

        if (state == PlayState.Play
            || state == PlayState.Selection)
        {
            input[frameCounter].Take(playerInfo.PlayerActions);
        }

        //Debug.LogFormat("[{0}][{1:f}] frameCounter {2} ", name, Time.fixedTime, frameCounter);


        InputSnapshot currentInput = input[frameCounter];
        if (state != PlayState.Selection)
            frameCounter++;

        isGrounded = BottomChecker.IsCollidingWith("Wall");

        if (isGrounded && currentInput.JumpWasPressed)
        {
            jumpUntil = Time.time + JumpTime;
        }

        if (currentInput.Aim.sqrMagnitude > 0.5f)
        {
            AimUpdate(currentInput.Aim);
        }

        MovementUpdate(currentInput.Move);

        if (currentInput.FireWasPressed)
        {
            //Debug.Log("Shoot !!!!!!!!!!!!!!");
            Shoot(state);
        }
    }

    private void MovementUpdate(float move)
    {
        var rigidbody = GetComponent<Rigidbody>();
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

    void AimUpdate(Vector2 aim)
    {
        Vector3 aimVec = new Vector3(aim.x, aim.y, 0).normalized;
        this.Aim.xLookAt(transform.position + aimVec);
    }

    private void Shoot(PlayState state)
    {
        var projGo = (GameObject)Instantiate(ProjPrefab, Nozzle.position, Nozzle.rotation);
        projGo.transform.parent = railShotsBase;
        RailShot railShot = projGo.GetComponent<RailShot>();
        railShot.Player = this;
        if (state == PlayState.Play)
        {
            railShot.Mark(Nozzle);
        }
        else
        {
            railShot.Shoot(Nozzle);
        }
    }
    
    void Die()
    {
        this.Body.GetComponent<BoxCollider>().enabled = false;
        this.Body.material.DOFade(0, 1f);
        this.Railgun.material.DOFade(0, 1f);
    }

    void Undie()
    {
        this.Body.GetComponent<BoxCollider>().enabled = true;
        this.Body.material.DOFade(1f, 0f);
        this.Railgun.material.DOFade(1f, 0f);
    }
    
    void OnGUI()
    {
        //var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //var screenHeight = Screen.height;

        //GUI.Label(new Rect(screenPos.x + 15, screenHeight - screenPos.y - 10, 100, 50), RightChecker.IsCollidingWith("Wall").ToString() );
        //GUI.Label(new Rect(screenPos.x - 50, screenHeight - screenPos.y - 10, 100, 50), LeftChecker.IsCollidingWith("Wall").ToString());
    }
}
