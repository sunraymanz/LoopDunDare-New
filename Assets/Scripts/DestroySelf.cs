﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public int destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
