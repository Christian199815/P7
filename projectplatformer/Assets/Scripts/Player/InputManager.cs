using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Local,
    MobileController
}
public class InputManager : MonoBehaviour
{
    public InputType inputType;
    public Vector2 axis;

    private void Update()
    {
        if (inputType == InputType.Local) axis = MovementAxis();
    }

    private Vector2 MovementAxis()
    {
        Vector2 inputAxis = Vector2.zero;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) inputAxis.x = -1;
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) inputAxis.x = 1;
        if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))) inputAxis.x = 0;
        inputAxis.x = Mathf.Clamp(inputAxis.x, -1f, 1f);
        inputAxis.y = Mathf.Clamp(inputAxis.y, -1f, 1f);
        return inputAxis;
    }
}
