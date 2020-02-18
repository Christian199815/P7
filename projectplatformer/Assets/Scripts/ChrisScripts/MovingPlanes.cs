using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlanes : MonoBehaviour
{
    public GameObject Plane;
    public int MovSpeed;

    

    
    // Update is called once per frame
    void Update()
    {
        Move();

        
    }

    void Move()
    {
        Plane.transform.position = new Vector3(Plane.transform.position.x - (MovSpeed * Time.smoothDeltaTime), Plane.transform.position.y, Plane.transform.position.z);
    }
}
