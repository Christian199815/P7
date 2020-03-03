using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public GameObject Player;
    public Camera cm;
    private bool isTopFloor = true;
    public int camSize = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
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
