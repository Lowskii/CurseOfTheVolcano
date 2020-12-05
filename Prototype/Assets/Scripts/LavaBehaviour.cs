﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private float _riseSpeed;
    [SerializeField] private float _MaxHeight;


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= _MaxHeight)
        {
            transform.Translate(new Vector3(0, _riseSpeed * Time.deltaTime, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}