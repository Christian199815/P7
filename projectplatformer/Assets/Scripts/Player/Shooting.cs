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
    public GameObject bulletTrail;
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
            }

            if (player.inputAxis == Vector2.zero)
            {
                gunObject.SetActive(false);
            }

            if (player.buttonAxis.z == 0)
            {
                if (gunObject.activeSelf)
                {
                    gunObject.SetActive(false);
                    Shoot();
                }
                player.movementEnabled = true;
            }
            yield return null;
        }
    }

    private void ShootingControlls()
    {
        Vector2 axis = player.inputAxis;//Controller.GetLeftStickAxis(1);
        //axis.y = -axis.y;

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
        if (shootingDirection.x > 0 && shootingDirection.y == 0) b = new Bullet(transform.position, new Vector3(1, 0, 0), bulletTrail);
        else if (shootingDirection.x < 0 && shootingDirection.y == 0) b = new Bullet(transform.position, new Vector3(-1, 0, 0), bulletTrail);
        if (shootingDirection.x == 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(0, 1, 0), bulletTrail);

        if (shootingDirection.x > 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(1, 1, 0), bulletTrail);
        if (shootingDirection.x < 0 && shootingDirection.y > 0) b = new Bullet(transform.position, new Vector3(-1, 1, 0), bulletTrail);
        
        
        //Nu kan je dingen opvragen met dit -> b?.hit.collider?.gameObject <-
    }
}
