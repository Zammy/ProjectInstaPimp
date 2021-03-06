﻿using InControl;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public Material Material;
    public PlayerActions PlayerActions;
    public InputDevice Device
    {
        get
        {
            return PlayerActions.Device;
        }
        set
        {
            PlayerActions.Device = value;
        }
    }
}

