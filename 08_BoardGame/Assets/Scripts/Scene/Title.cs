using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Title.Enable();
        inputActions.Title.Anything.performed += OnAnythingPress;
    }

    private void OnDisable()
    {
        inputActions.Title.Anything.performed -= OnAnythingPress;
        inputActions.Title.Disable(); 
    }

    private void OnAnythingPress(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(1);
    }
}
