using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Users
{
    public string userName;
    public int userScore;

    public Users()
    {
        userName = pausee.playername;
        userScore = pausee.playerscore;
    }
}
