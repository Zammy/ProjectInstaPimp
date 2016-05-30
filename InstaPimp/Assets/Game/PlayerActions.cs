using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Fire;
    public PlayerAction Jump;

    public PlayerAction MoveLeft;
    public PlayerAction MoveRight;

    public PlayerOneAxisAction Move;

    public PlayerAction AimLeft;
    public PlayerAction AimRight;
    public PlayerAction AimUp;
    public PlayerAction AimDown;
    public PlayerTwoAxisAction Aim;

    public PlayerActions()
    {
        Fire = CreatePlayerAction("Fire");
        Jump = CreatePlayerAction("Jump");

        MoveLeft = CreatePlayerAction("Move Left");
        MoveRight = CreatePlayerAction("Move Right");

        Move = CreateOneAxisPlayerAction(MoveLeft, MoveRight);

        AimLeft = CreatePlayerAction("Aim Left");
        AimRight = CreatePlayerAction("Aim Right");
        AimUp = CreatePlayerAction("Aim Up");
        AimDown = CreatePlayerAction("Aim Down");
        Aim = CreateTwoAxisPlayerAction(AimLeft, AimRight, AimDown, AimUp);
    }


    public static PlayerActions CreateWithDefaultBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Fire.AddDefaultBinding(InputControlType.RightTrigger);
        //playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);

        //playerActions.Jump.AddDefaultBinding(Key.W);
        //playerActions.Jump.AddDefaultBinding(Key.Space);
        playerActions.Jump.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Jump.AddDefaultBinding(InputControlType.LeftTrigger);

        //move
        //playerActions.MoveLeft.AddDefaultBinding(Key.A);
        //playerActions.MoveRight.AddDefaultBinding(Key.D);
        //playerActions.MoveLeft.AddDefaultBinding(Key.LeftArrow);
        //playerActions.MoveRight.AddDefaultBinding(Key.RightArrow);

        playerActions.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);

        playerActions.MoveLeft.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.MoveRight.AddDefaultBinding(InputControlType.DPadRight);

        //aim
        playerActions.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        playerActions.AimRight.AddDefaultBinding(InputControlType.RightStickRight);
        playerActions.AimUp.AddDefaultBinding(InputControlType.RightStickUp);
        playerActions.AimDown.AddDefaultBinding(InputControlType.RightStickDown);

        //playerActions.AimUp.AddDefaultBinding(Mouse.PositiveY);
        //playerActions.AimDown.AddDefaultBinding(Mouse.NegativeY);
        //playerActions.AimLeft.AddDefaultBinding(Mouse.NegativeX);
        //playerActions.AimRight.AddDefaultBinding(Mouse.PositiveX);

        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 4;
        //			playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
        //			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        //			playerActions.ListenOptions.IncludeMouseButtons = true;

        playerActions.ListenOptions.OnBindingFound = (action, binding) =>
        {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerActions.ListenOptions.OnBindingAdded += (action, binding) =>
        {
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
        };

        playerActions.ListenOptions.OnBindingRejected += (action, binding, reason) =>
        {
            Debug.Log("Binding rejected... " + reason);
        };

        return playerActions;
    }
}

