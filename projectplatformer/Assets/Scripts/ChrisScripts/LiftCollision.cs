﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollision : MonoBehaviour
{
    public Vector3 LiftEndPos;
    public float LiftDownSpeed;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            transform.position = Vector3.Lerp(transform.position, LiftEndPos, Mathf.SmoothStep(0, 1, 12));
        }
    }

}
