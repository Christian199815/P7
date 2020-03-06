using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiButton : MonoBehaviour
{
    public bool Start;
    public bool Quit;
    
    public void OnButtonDown()
    {
        if (Start)
        {
            SceneManager.LoadScene(1);
        }
        else if (Quit)
        {
            Application.Quit();
        }
    }
}
