﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float Speed;
    void Update()
    {
        this.transform.position += Vector3.up * Speed * Time.deltaTime;
    }
}
