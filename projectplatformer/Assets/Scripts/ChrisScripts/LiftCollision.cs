using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollision : MonoBehaviour
{
    private Vector3 LiftEndPos;
    public float LiftDownSpeed;
    public GameObject Player;
    public int MinLifPos;

    public bool PlayerDetected = false;


    private void Start()
    {
        LiftEndPos = new Vector3(40.56f, -27.25f, transform.position.z);
    }

    private void Update()
    {
        PlayerOnLift();

        if (PlayerDetected == true)
        {
             transform.position = Vector3.Lerp(transform.position, LiftEndPos, (LiftDownSpeed * Time.deltaTime));
        }

        
       
    }

    private void PlayerOnLift()
    {
        if(Player.transform.position.x >= MinLifPos)
        {
            PlayerDetected = true;
        }
    }

}
