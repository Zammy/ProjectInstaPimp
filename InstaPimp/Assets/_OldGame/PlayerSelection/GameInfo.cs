using System.Collections.Generic;

public enum GameMode
{
    Deathmatch = 0,
    LastManStanding = 1,
}

public static class GameInfo
{
    public static List<PlayerInfo> Players;
    public static GameMode GameMode = GameMode.Deathmatch;

    public static int DeathmatchFragGoal = 10;
}