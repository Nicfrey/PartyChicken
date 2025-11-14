using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;

public static class EnumToString
{
    public static string GetGameModeDescription(this GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.FFA:
                return "Free For All";
            case GameMode.CrownChase:
                return "Crown Chase";
            case GameMode.KingOfTheHill:
                return "King Of The Hill";
            default:
                throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
        }
    }
}
