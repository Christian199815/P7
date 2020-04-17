using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selektorhandler : MonoBehaviour
{
    public Image Selector;
    private int numStart = 32;
    private int numQuit = -173;

    private int SelektorID;

    public InputManager Iman;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        Iman = FindObjectOfType<InputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        MoveSelektor();
        select();

       
    }

    private void select()
    {
        if (Iman == null)
            return;
        //druk op knop en plaatje is voor de start tekst
        if(Iman.buttonAxis.x == 1 && SelektorID == 0)
        {
            print("buttonpressed");
            SceneManager.LoadScene(2);
        }
        //druk op de knop en plaatje is voor de quit tekst
        else if(Iman.buttonAxis.x == 1 && SelektorID == 1)
        {
            Application.Quit();
        }
    }

    private void MoveSelektor()
    {
        if (Iman == null)
            return;
        if(Iman.axis.y >= 1 && SelektorID == 1)
        {
            SelektorID = 0;
            Selector.rectTransform.localPosition = new Vector3(-940, 32, 0);
        }
        else if(Iman.axis.y <= -1 && SelektorID == 0)
        {
            SelektorID = 1;
            Selector.rectTransform.localPosition = new Vector3(-940, -173, 0);
           
        }
    }


}
