using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selektorhandler : MonoBehaviour
{
    public Image Selektor;
    private int numStart = 32;
    private int numQuit = -173;

    public InputManager Iman;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveSelektor();
        select();
    }

    private void select()
    {
        //druk op knop en plaatje is voor de start tekst
        if(Iman.buttonAxis.x == 1 && Selektor.transform.position.y == 32)
        {
            SceneManager.LoadScene(2);
        }
        //druk op de knop en plaatje is voor de quit tekst
        else if(Iman.buttonAxis.x == 1 && Selektor.transform.position.y == -173)
        {
            Application.Quit();
        }
    }

    private void MoveSelektor()
    {
        if(Iman.axis.y == 1 && Selektor.transform.position.y == -173)
        {
            Selektor.transform.position = new Vector3(67, 32, 0);
        }
        else if(Iman.axis.y == -1 && Selektor.transform.position.y == 32)
        {
            Selektor.transform.position = new Vector3(67, -173, 0);
        }
    }


}
