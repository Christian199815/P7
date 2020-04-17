using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public GameObject Lift;
    private Vector3 EndPos;

    private void Start()
    {
        EndPos = new Vector3(40.56f, -25.25f, Lift.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(Lift.transform.position.y <= EndPos.y)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        
        FindObjectOfType<FinishedClient>().SendMessageToServer("HOME");
        SceneManager.LoadScene("Home");
    }
}
