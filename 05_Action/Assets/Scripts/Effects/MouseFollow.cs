using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Effect.Enable();
        inputActions.Effect.PointerMove.performed += OnPointerMove;
    }

    private void OnDisable()
    {
        inputActions.Effect.PointerMove.performed -= OnPointerMove;
        inputActions.Effect.Disable();
    }

    private void OnPointerMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();
        mousePos.z = 10.0f;

        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = target;
    }
}
