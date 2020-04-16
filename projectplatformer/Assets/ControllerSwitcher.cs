using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<InputManager>()?.SendGameScreenMessage();
    }
}
