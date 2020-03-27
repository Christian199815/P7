using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject HiddenPlatform;
    public float speed;
    private Vector3 HiddenPlatformDesPos;
    private Vector3 HiddenPlatformOrgPos;

    public bool OnPressurePlate = false;
    public bool NotOnPressurePlateAnymore = false;


    private void Start()
    {
        HiddenPlatformOrgPos = HiddenPlatform.transform.position;
        HiddenPlatformDesPos = new Vector3(28.384f, 6.73f, HiddenPlatform.transform.position.z);
    }


    private void Update()
    {
        if(OnPressurePlate == true)
        {
            MovePlatformToDes();
        }

        //if(NotOnPressurePlateAnymore == false)
        //{
        //    MovePlatformBack();
        //}
    }


    private void MovePlatformToDes()
    {
        HiddenPlatform.transform.position = Vector3.Lerp(HiddenPlatform.transform.position, HiddenPlatformDesPos, (speed * Time.deltaTime));
    }

    private void MovePlatformBack()
    {
        if (HiddenPlatform.transform.position != HiddenPlatformOrgPos)
        {
            HiddenPlatform.transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box"))
        {
            if(OnPressurePlate == false)
            {
                Debug.Log("Input Detected!");
                OnPressurePlate = true;
                //NotOnPressurePlateAnymore = false;
            }
           
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(OnPressurePlate == true)
            {
                Debug.Log("Input Lost");
                NotOnPressurePlateAnymore = true;
               // OnPressurePlate = false;
            }
            
        }
    }


}
