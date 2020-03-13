using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Shooting : MonoBehaviour
{
    private Player player;
    public bool shootingEnabled;
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
                player.movementEnabled = false;
            }
            else
            {
                player.movementEnabled = true;
            }
                yield return null;
        }
    }
}
