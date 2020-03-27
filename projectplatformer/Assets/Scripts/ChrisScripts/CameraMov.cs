using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public GameObject Player;



    private void Start()
    {
        
    }

    private void Update()
    {
        if (Player.transform.position.x >= -46.7f && Player.transform.position.x <= 11.6)
        {
            Section1();
        }
        else if (Player.transform.position.x >= 11.7 && Player.transform.position.x <= 40.5f)
        {
            Section2();
        }
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
