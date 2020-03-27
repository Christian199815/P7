using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollision : MonoBehaviour
{
    private Vector3 LiftEndPos;
    public float LiftDownSpeed;

    public bool PlayerDetected = false;


    private void Start()
    {
        LiftEndPos = new Vector3(40.56f, -27.25f, transform.position.z);
    }

    private void Update()
    {
        if (PlayerDetected == true)
        {
             transform.position = Vector3.Lerp(transform.position, LiftEndPos, (LiftDownSpeed * Time.deltaTime));
        }

        if(transform.position == LiftEndPos)
        {
            PlayerDetected = false;
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerDetected = true;
        }
    }

}
