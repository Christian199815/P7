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
            }
            else
            {
                if (gunObject.activeSelf) gunObject.SetActive(false);
                player.movementEnabled = true;
            }
                yield return null;
        }
    }

    private void ShootingControlls()
    {
        Vector2 axis = player.inputAxis;

        if (axis.x > 0 && axis.y == 0) shootingDirection = new Vector2(1, 0);
        else if (axis.x < 0 && axis.y == 0) shootingDirection = new Vector2(-1, 0);
        else if (axis.x == 0 && axis.y > 0) shootingDirection = new Vector2(0, 1);
        else if (axis.x > 0 && axis.y > 0) shootingDirection = new Vector2(1, 1);
        else if (axis.x < 0 && axis.y > 0) shootingDirection = new Vector2(-1, 1);

        if (axis.x == 0 && axis.y == 0) shootingDirection = Vector2.zero;
    }
}
