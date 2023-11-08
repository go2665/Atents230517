using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : MonoBehaviour
{
    public Color baseColor;

    public float moveSpeed = 1.0f;
    public float rotateSpeed = 360.0f;
    public float turretSpinSpeed = 4.0f;

    Vector2 inputDir = Vector2.zero;
    Quaternion lookTarget = Quaternion.identity;

    Rigidbody rigid;
    Transform turret;

    PlayerInputActions inputActions;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        Transform child = transform.GetChild(0);
        turret = child.GetChild(3).transform;
        lookTarget = turret.rotation;
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

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();        
    }

    private void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.SetColor("_BaseColor", baseColor);
        }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * inputDir.y * transform.forward);
        rigid.MoveRotation(
            Quaternion.Euler(0, Time.fixedDeltaTime * rotateSpeed * inputDir.x, 0) * transform.rotation);
    }

    private void Update()
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if( Physics.Raycast(ray, out RaycastHit hitInfo, 100.0f, LayerMask.GetMask("Ground")) )
        {
            Vector3 lookDir = hitInfo.point - turret.position;
            lookDir.y = 0;
            lookTarget = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        turret.rotation = Quaternion.Slerp(turret.rotation, lookTarget, Time.deltaTime * turretSpinSpeed);
    }
}
