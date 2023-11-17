using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_231116 : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float turnSpeed = 180.0f;

    PlayerInputActions_231116 inputActions;
    Vector2 input;

    private void Awake()
    {
        inputActions = new PlayerInputActions_231116();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.Translate(input.y * moveSpeed * Time.deltaTime * transform.forward, Space.World);
        transform.Rotate(Time.deltaTime * turnSpeed * input.x * transform.up);
    }

}
