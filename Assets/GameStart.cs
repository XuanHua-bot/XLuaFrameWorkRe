﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    public GameMode GameMode;
   
    private void Awake()
    {
        AppConst.Gamemode = this.GameMode;

    }

    
}
