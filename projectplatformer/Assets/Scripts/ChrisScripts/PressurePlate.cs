using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject HiddenPlatform;
    public float speed;
    private Vector3 HiddenPlatformOrgPos;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box"))
        {
            HiddenPlatform.transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (HiddenPlatform.transform.position != HiddenPlatformOrgPos)
            {
                HiddenPlatform.transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
            }
        }
    }


}
