using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public GameObject Player;
    public Camera cm;
    private bool isTopFloor = true;
    public int camSize = 15;
    public Vector3 offset;
    public float moveSpeed;



    private void Start()
    {

    }

    private void Update()
    {
        if (Player.transform.position.y > 3.9f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x + offset.x, 9.2f, Player.transform.position.z + offset.z), Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x + offset.x, 4.5f, Player.transform.position.z + offset.z), Time.deltaTime * moveSpeed);
        }

        CameraAndLift();
    }

    private void Section1()
    {
        transform.position = new Vector3(Player.transform.position.x, 3.7f, 3.5f);
    }

    private void Section2()
    {
        if(Player.transform.position.y >= 2 && Player.transform.position.y <= 10)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, 3.5f);
        }
        else
        {
            transform.position = new Vector3(Player.transform.position.x, 10.2f, 3.5f);
        }
    }

    private void Section3()
    {

    }
}
