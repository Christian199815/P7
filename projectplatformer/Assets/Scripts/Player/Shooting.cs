using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Shooting : MonoBehaviour
{
    private Player player;
    public bool shootingEnabled;
    public Vector2 shootingDirection;
    public GameObject gunObject;
    void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(ShootingController());
    }
    
    
    private IEnumerator ShootingController()
    {
        while (true)
        {
            if (player.buttonAxis.z == 1)
            {
                if (!gunObject.activeSelf) gunObject.SetActive(true);
                player.movementEnabled = false;
                ShootingControlls();
                yield return null;
                if (player.buttonAxis.z == 0)
                {
                    Shoot();
                    if (gunObject.activeSelf) gunObject.SetActive(false);
                    player.movementEnabled = true;
                }
            }
            yield return null;
        }
    }

    private void ShootingControlls()
    {
        Vector2 axis = player.inputAxis;//Controller.GetLeftStickAxis(1);
        axis.y = -axis.y;

        if (axis.x > 0 && axis.y == 0) shootingDirection = new Vector2(1, 0);
        else if (axis.x < 0 && axis.y == 0) shootingDirection = new Vector2(-1, 0);
        else if (axis.x == 0 && axis.y > 0) shootingDirection = new Vector2(0, 1);
        else if (axis.x > 0 && axis.y > 0) shootingDirection = new Vector2(1, 1);
        else if (axis.x < 0 && axis.y > 0) shootingDirection = new Vector2(-1, 1);

        if (axis.x == 0 && axis.y == 0) shootingDirection = Vector2.zero;

        if (shootingDirection.x > 0 && shootingDirection.y == 0) gunObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (shootingDirection.x < 0 && shootingDirection.y == 0) gunObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        if (shootingDirection.x == 0 && shootingDirection.y > 0) gunObject.transform.rotation = Quaternion.Euler(0, 0, 90);

        if (shootingDirection.x > 0 && shootingDirection.y > 0) gunObject.transform.rotation = Quaternion.Euler(0, 0, 45);
        if (shootingDirection.x < 0 && shootingDirection.y > 0) gunObject.transform.rotation = Quaternion.Euler(0, 0, 135);
    }

    private void Shoot()
    {
        Bullet b = null;
        if (shootingDirection.x > 0 && shootingDirection.y == 0) b = new Bullet(transform.position, new Vector3(1, 0, 0));
        else if (shootingDirection.x < 0 && shootingDirection.y == 0) b = new Bullet(transform.position, new Vector3(-1, 0, 0));
        if (shootingDirection.x == 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(0, 1, 0));

        if (shootingDirection.x > 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(1, 1, 0));
        if (shootingDirection.x < 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(-1, 1, 0));

        
        print(b?.hit.collider?.gameObject.name);
    }
}
