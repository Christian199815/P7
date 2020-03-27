using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    Vector3 direction;
    Ray ray;
    public RaycastHit2D hit;
    GameObject bulletTrail;
    float trailSpeed = 80;

    public Bullet(Vector3 _startPos, Vector3 _direction, GameObject _bulletTrail)
    {
        startPos = _startPos;
        direction = _direction;
        Debug.DrawLine(_startPos, _startPos + _direction * 50, Color.red, 1);
        bulletTrail = _bulletTrail;
        Shoot();
        Visualize();
    }

    private void Shoot()
    {
        RaycastHit2D[] _hit = Physics2D.RaycastAll(startPos, direction);
        //Returnt 1, want 0 is de player zelf
        if (_hit.Length > 1)
        {
            if (_hit[1] == true)
            {
                hit = _hit[1];
                return;
            }
        }
        //Bullet hit nothing

    }

    private void Visualize()
    {
        GameObject b = Instantiate(bulletTrail, startPos + new Vector3(0, 0, 1), Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = direction * trailSpeed;
        Destroy(b, 2);
    }
}
