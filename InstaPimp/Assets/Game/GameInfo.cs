using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameInfo
{
    public static List<PlayerInfo> Players;
    public static List<int> ShootFrames = new List<int>()
    {
        60, 120, 180
    };
}