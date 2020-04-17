using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInput : MonoBehaviour
{
    public Vector2 axis = new Vector2();
    public Vector4 buttons = new Vector4();

    [SerializeField] private Button up;
    [SerializeField] private Button right;
    [SerializeField] private Button down;
    [SerializeField] private Button left;

    [SerializeField] private Button jump;
    [SerializeField] private Button GHook;
    [SerializeField] private Button shoot;
    [SerializeField] private Button sprint;

    [SerializeField] private Button OKbutton;
    [SerializeField] private Button arrowDown;
    [SerializeField] private Button arrowUp;


    public FinishedClient client;

    private bool buttonUpPressed = false, buttonRightPressed = false, buttonDownPressed = false, buttonLeftPressed = false;
    private bool buttonJumpPressed = false, buttonGHookPressed = false, buttonShootPressed = false, buttonSprintPressed = false, buttonOKpressed = false;
    private void Start()
    {
        client = FindObjectOfType<FinishedClient>();
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

        if (buttonJumpPressed || buttonOKpressed) buttons.x = 1;
        else if (!buttonJumpPressed && !buttonOKpressed) buttons.x = 0;

        if (buttonGHookPressed) buttons.y = 1;
        else if (!buttonGHookPressed) buttons.y = 0;

        if (buttonShootPressed) buttons.z = 1;
        else if (!buttonShootPressed) buttons.z = 0;

        if (buttonSprintPressed) buttons.w = 1;
        else if (!buttonSprintPressed) buttons.w = 0;

        
    }

    public void PointerDown(Button button)
    {
        if (button == up || button == arrowUp) buttonUpPressed = true;
        else if (button == right) buttonRightPressed = true;
        else if (button == down || button == arrowDown) buttonDownPressed = true;
        else if (button == left) buttonLeftPressed = true;

        else if (button == jump) buttonJumpPressed = true;
        else if (button == GHook) buttonGHookPressed = true;
        else if (button == shoot) buttonShootPressed = true;
        else if (button == sprint) buttonSprintPressed = true;

        else if (button == OKbutton) buttonOKpressed = true;
        

        ChangeAxis();

        SendData();
    }

    public void PointerUp(Button button)
    {
        if (button == up || button == buttonUpPressed) buttonUpPressed = false;
        else if (button == right) buttonRightPressed = false;
        else if (button == down || button == buttonDownPressed) buttonDownPressed = false;
        else if (button == left) buttonLeftPressed = false;

        else if (button == jump) buttonJumpPressed = false;
        else if (button == GHook) buttonGHookPressed = false;
        else if (button == shoot) buttonShootPressed = false;
        else if (button == sprint) buttonSprintPressed = false;

        else if (button == OKbutton) buttonOKpressed = false;


        ChangeAxis();

        SendData();
    }

    private void SendData()
    {
        if (client.PairedToGame)
        {
            client.SendMessageToServer(axis.x + "/" + axis.y + "/" + buttons.x + "/" + buttons.y + "/" + buttons.z + "/" + buttons.w);
        }
        
    }
}
