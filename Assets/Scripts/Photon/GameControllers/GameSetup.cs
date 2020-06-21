﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup gameSetup;
    public Transform[] spawnPoints;

    private void OnEnable()
    {
        if (gameSetup == null)
        {
            gameSetup = this;
        }
    }
}
