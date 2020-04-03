using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InputType
{
    LocalKeyboard,
    LocalController,
    MobileController,
}
public class InputManager : MonoBehaviour
{
    public InputType inputType;
    public Vector2 axis;
    public Vector4 buttonAxis;

    public static InputManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (inputType == InputType.LocalKeyboard)
        {
            axis = MovementAxis();
            buttonAxis = MovementButtons();
        }
        else if (inputType == InputType.LocalController)
        {
            axis = ControllerAxis();

            if (Controller.GetButtonB(0)) buttonAxis.x = 1;
            else buttonAxis.x = 0;
            if (Controller.GetButtonY(0)) buttonAxis.y = 1;
            else buttonAxis.y = 0;
            if (Controller.GetButtonX(0)) buttonAxis.z = 1;
            else buttonAxis.z = 0;
            if (Controller.GetButtonA(0)) buttonAxis.w = 1;
            else buttonAxis.w = 0;

        }
    }

    private Vector4 MovementButtons()
    {
        Vector4 buttonAxis = Vector4.zero;
        if (Input.GetKey(KeyCode.Space)) buttonAxis.x = 1;
        else if (!Input.GetKey(KeyCode.Space)) buttonAxis.x = 0;

        buttonAxis.x = Mathf.Clamp(buttonAxis.x, -1f, 1f);

        return buttonAxis;
    }
    private Vector2 MovementAxis()
    {
        Vector2 inputAxis = Vector2.zero;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) inputAxis.x = -1;
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) inputAxis.x = 1;
        if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))) inputAxis.x = 0;

        return inputAxis;
    }

    private Vector2 ControllerAxis()
    {
        return Controller.GetDpadAxis(0);
    }

    public void SetInputMode(int mode)
    {
        switch (mode)
        {
            case 0:
                inputType = InputType.LocalKeyboard;
                break;
            case 1:
                inputType = InputType.LocalController;
                break;
            case 2:
                inputType = InputType.MobileController;
                break;
        }   
    }

    public void SwitchToGameScene()
    {
        SceneManager.LoadSceneAsync("BlockUpHomeWorld");
    }
}
