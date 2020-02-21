using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject HiddenPlatform;
    public float speed;
    private Vector3 HiddenPlatformDesPos;
    private Vector3 HiddenPlatformOrgPos;

    private void Start()
    {
        HiddenPlatformOrgPos = HiddenPlatform.transform.position;
        HiddenPlatformDesPos = new Vector3(39.8f, 1.5f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box"))
        {
            Debug.Log("Input Detected!");
            HiddenPlatform.transform.position = Vector3.Lerp(HiddenPlatform.transform.position, HiddenPlatformDesPos, (speed * Time.deltaTime));
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
