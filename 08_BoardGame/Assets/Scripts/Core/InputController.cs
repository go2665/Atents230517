using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<Vector2> onMouseClick;
    public Action<Vector2> onMouseMove;
    public Action<float> onMouseWheel;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MouseClick.performed += OnMouseClick;
        inputActions.Player.MouseMove.performed += OnMouseMove;
        inputActions.Player.MouseWheel.performed += OnMouseWheel;
    }

    private void OnDisable()
    {
        inputActions.Player.MouseWheel.performed -= OnMouseWheel;
        inputActions.Player.MouseMove.performed -= OnMouseMove;
        inputActions.Player.MouseClick.performed -= OnMouseClick;
        inputActions.Player.Disable();
    }

    private void OnMouseClick(InputAction.CallbackContext _)
    {
        onMouseClick?.Invoke(Mouse.current.position.ReadValue());
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        onMouseMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMouseWheel(InputAction.CallbackContext context)
    {
        onMouseWheel?.Invoke(context.ReadValue<float>());
    }

    public void ResetBind()
    {
        onMouseClick = null;
        onMouseMove = null;
        onMouseWheel = null;
    }
}
