using System.Collections.Generic;

public enum GameMode
{
    Deathmatch,
    LastManStanding,
}

public static class GameInfo
{
    public static List<PlayerInfo> Players;
    public static GameMode GameMode;
}