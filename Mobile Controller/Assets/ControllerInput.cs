using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInput : MonoBehaviour
{
    public Vector2 axis = new Vector2();

    [SerializeField] private Button up;
    [SerializeField] private Button right;
    [SerializeField] private Button down;
    [SerializeField] private Button left;

    private Client client;

    private bool buttonUpPressed = false, buttonRightPressed = false, buttonDownPressed = false, buttonLeftPressed = false;
    private void Start()
    {
        client = FindObjectOfType<Client>();
    }
    void ChangeAxis()
    {
        //X
        if (buttonRightPressed && !buttonLeftPressed) axis.x = 1;
        if (!buttonRightPressed && buttonLeftPressed) axis.x = -1;
        if ((buttonRightPressed && buttonLeftPressed) || (!buttonRightPressed && !buttonLeftPressed)) axis.x = 0;

        //Y
        if (buttonUpPressed && !buttonDownPressed) axis.y = 1;
        if (!buttonUpPressed && buttonDownPressed) axis.y = -1;
        if ((buttonUpPressed && buttonDownPressed) || (!buttonUpPressed && !buttonDownPressed)) axis.y = 0;
    }

    public void PointerDown(Button button)
    {
        if (button == up) buttonUpPressed = true;
        else if (button == right) buttonRightPressed = true;
        else if (button == down) buttonDownPressed = true;
        else if (button == left) buttonLeftPressed = true;

        ChangeAxis();

        SendData();
    }

    public void PointerUp(Button button)
    {
        if (button == up) buttonUpPressed = false;
        else if (button == right) buttonRightPressed = false;
        else if (button == down) buttonDownPressed = false;
        else if (button == left) buttonLeftPressed = false;

        ChangeAxis();

        SendData();
    }

    private void SendData()
    {
        client.SendMessageToServer(axis.x + "/" + axis.y);
    }
}
