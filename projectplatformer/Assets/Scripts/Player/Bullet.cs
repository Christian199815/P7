using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    Quaternion direction;
    Ray ray;
    public RaycastHit hit;

    public Bullet(Vector3 _startPos, Vector3 _direction)
    {
        ray = new Ray(_startPos, _direction);
        Debug.DrawLine(_startPos, _startPos + _direction * 10, Color.red, 1);
        Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(ray, out hit))
        {

        }
        else print("bullet hit nothing!");
    }
}
