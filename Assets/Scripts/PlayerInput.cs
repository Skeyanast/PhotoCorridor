using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool _moveForwardPressed;
    private bool _moveBackwardPressed;
    private bool _rotateLeftPressed;
    private bool _rotateRightPressed;

    public event Action MoveForwardPressed;
    public event Action MoveBackwardPressed;
    public event Action RotateLeftPressed;
    public event Action RotateRightPressed;

    private void Update()
    {
        GetInput();
        InvokeEvents();
    }

    private void GetInput()
    {
        _moveForwardPressed = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow);
        _moveBackwardPressed = Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow);
        _rotateLeftPressed = Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow);
        _rotateRightPressed = Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow);
    }

    private void InvokeEvents()
    {
        if (_moveForwardPressed) MoveForwardPressed?.Invoke();
        if (_moveBackwardPressed) MoveBackwardPressed?.Invoke();
        if (_rotateLeftPressed) RotateLeftPressed?.Invoke();
        if (_rotateRightPressed) RotateRightPressed?.Invoke();
    }
}
