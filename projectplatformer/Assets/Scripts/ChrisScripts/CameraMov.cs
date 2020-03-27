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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    void CameraAndLift()
    {
        if (transform.position.x >= 63f && isTopFloor)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -27.5f, transform.position.z), 2f * Time.deltaTime);
            cm.orthographicSize = 15;
            isTopFloor = false;
        }
    }
}
