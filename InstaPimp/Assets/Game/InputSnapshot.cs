using UnityEngine;

public struct InputSnapshot
{
    public float Timestamp;
    public bool JumpWasPressed;
    public bool FireWasPressed;
    public float Move;
    public Vector2 Aim;

    public void Take(PlayerActions playerActions)
    {
        Timestamp = Time.time;
        JumpWasPressed = playerActions.Jump.WasPressed;
        //    FireWasPressed = playerActions.Fire.WasPressed;
        Move = playerActions.Move.Value;
        Aim = new Vector2(playerActions.Aim.X, playerActions.Aim.Y);
    }

    public override string ToString()
    {
        return string.Format("[{0:f}] Jump({1}) Fire({2}) Move({3}) Aim({4}) ", Timestamp, JumpWasPressed, FireWasPressed, Move, Aim);
    }
}
