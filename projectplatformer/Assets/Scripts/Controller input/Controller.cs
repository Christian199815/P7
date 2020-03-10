using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controller
{
    #region buttons

    #region buttonA
    public static bool GetButtonDownA(int controllerNumber)
    {  
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button0);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button0);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button0);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button0);
        }
        return false;
    }

    public static bool GetButtonA(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button0);
            case 2: return Input.GetKey(KeyCode.Joystick2Button0);
            case 3: return Input.GetKey(KeyCode.Joystick3Button0);
            case 4: return Input.GetKey(KeyCode.Joystick4Button0);
        }
        return false;
    }
    #endregion

    #region buttonB
    public static bool GetButtonDownB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button1);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button1);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button1);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button1);
        }
        return false;
    }

    public static bool GetButtonB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button1);
            case 2: return Input.GetKey(KeyCode.Joystick2Button1);
            case 3: return Input.GetKey(KeyCode.Joystick3Button1);
            case 4: return Input.GetKey(KeyCode.Joystick4Button1);
        }
        return false;
    }
    #endregion

    #region buttonX
    public static bool GetButtonDownX(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button2);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button2);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button2);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button2);
        }
        return false;
    }

    public static bool GetButtonX(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button2);
            case 2: return Input.GetKey(KeyCode.Joystick2Button2);
            case 3: return Input.GetKey(KeyCode.Joystick3Button2);
            case 4: return Input.GetKey(KeyCode.Joystick4Button2);
        }
        return false;
    }
    #endregion

    #region buttonY
    public static bool GetButtonDownY(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button3);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button3);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button3);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button3);
        }
        return false;
    }

    public static bool GetButtonY(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button3);
            case 2: return Input.GetKey(KeyCode.Joystick2Button3);
            case 3: return Input.GetKey(KeyCode.Joystick3Button3);
            case 4: return Input.GetKey(KeyCode.Joystick4Button3);
        }
        return false;
    }
    #endregion

    #region leftBumper
    public static bool GetButtonDownLB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button4);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button4);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button4);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button4);
        }
        return false;
    }

    public static bool GetButtonLB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button4);
            case 2: return Input.GetKey(KeyCode.Joystick2Button4);
            case 3: return Input.GetKey(KeyCode.Joystick3Button4);
            case 4: return Input.GetKey(KeyCode.Joystick4Button4);
        }
        return false;
    }
    #endregion

    #region rightBumper
    public static bool GetButtonDownRB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKeyDown(KeyCode.Joystick1Button5);
            case 2: return Input.GetKeyDown(KeyCode.Joystick2Button5);
            case 3: return Input.GetKeyDown(KeyCode.Joystick3Button5);
            case 4: return Input.GetKeyDown(KeyCode.Joystick4Button5);
        }
        return false;
    }

    public static bool GetButtonRB(int controllerNumber)
    {

        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetKey(KeyCode.Joystick1Button5);
            case 2: return Input.GetKey(KeyCode.Joystick2Button5);
            case 3: return Input.GetKey(KeyCode.Joystick3Button5);
            case 4: return Input.GetKey(KeyCode.Joystick4Button5);
        }
        return false;
    }
    #endregion

    #region getAnyButtonDown
    public static bool GetAllButtonsDownA()
    {
        if (GetButtonA(1) || GetButtonA(2) || GetButtonA(3) || GetButtonA(4))
        {
            return true;
        }
        return false;
    }
    public static bool GetAllButtonsDownB()
    {
        if (GetButtonB(1) || GetButtonB(2) || GetButtonB(3) || GetButtonB(4))
        {
            return true;
        }
        return false;
    }
    public static bool GetAllButtonsDownX()
    {
        if (GetButtonX(1) || GetButtonX(2) || GetButtonX(3) || GetButtonX(4))
        {
            return true;
        }
        return false;
    }
    public static bool GetAllButtonsDownY()
    {
        if (GetButtonY(1) || GetButtonY(2) || GetButtonY(3) || GetButtonY(4))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region returnJoysticks
    public static string[] getActiveControllers()
    {
        return Input.GetJoystickNames();
    }
    #endregion

    #endregion

    #region axises

        public static Vector2 GetLeftStickAxis(int controllerNumber)
    {
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return new Vector2(Input.GetAxis("Joy1LeftStickX"), Input.GetAxis("Joy1LeftStickY"));
            case 2: return new Vector2(Input.GetAxis("Joy2LeftStickX"), Input.GetAxis("Joy2LeftStickY"));
            case 3: return new Vector2(Input.GetAxis("Joy3LeftStickX"), Input.GetAxis("Joy3LeftStickY"));
            case 4: return new Vector2(Input.GetAxis("Joy4LeftStickX"), Input.GetAxis("Joy4LeftStickY"));
        }
        return new Vector2();
    }

    public static Vector2 GetRightStickAxis(int controllerNumber)
    {
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return new Vector2(Input.GetAxis("Joy1RightStickX"), Input.GetAxis("Joy1RightStickY"));
            case 2: return new Vector2(Input.GetAxis("Joy2RightStickX"), Input.GetAxis("Joy2RightStickY"));
            case 3: return new Vector2(Input.GetAxis("Joy3RightStickX"), Input.GetAxis("Joy3RightStickY"));
            case 4: return new Vector2(Input.GetAxis("Joy4RightStickX"), Input.GetAxis("Joy4RightStickY"));
        }
        return new Vector2();
    }

    public static Vector2 GetDpadAxis(int controllerNumber)
    {
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return new Vector2(Input.GetAxis("Joy1DpadX"), Input.GetAxis("Joy1DpadY"));
            case 2: return new Vector2(Input.GetAxis("Joy2DpadX"), Input.GetAxis("Joy2DpadY"));
            case 3: return new Vector2(Input.GetAxis("Joy3DpadX"), Input.GetAxis("Joy3DpadY"));
            case 4: return new Vector2(Input.GetAxis("Joy4DpadX"), Input.GetAxis("Joy4DpadY"));
        }
        return new Vector2();
    }

    public static float GetLeftTrigger(int controllerNumber)
    {
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetAxis("Joy1LT");
            case 2: return Input.GetAxis("Joy2LT");
            case 3: return Input.GetAxis("Joy3LT");
            case 4: return Input.GetAxis("Joy4LT");
        }
        return 0f;
    }

    public static float GetRightTrigger(int controllerNumber)
    {
        switch (Mathf.Clamp(controllerNumber, 1, 4))
        {
            case 1: return Input.GetAxis("Joy1RT");
            case 2: return Input.GetAxis("Joy2RT");
            case 3: return Input.GetAxis("Joy3RT");
            case 4: return Input.GetAxis("Joy4RT");
        }
        return 0f;
    }


    #endregion
}
