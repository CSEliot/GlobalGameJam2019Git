﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonArenaManager.instance.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
