using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Tank : PlayerBase
{    
    public float turretSpinSpeed = 4.0f;
    
    Quaternion lookTarget = Quaternion.identity;
    
    Transform turret;    

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(0);
        turret = child.GetChild(3).transform;
        lookTarget = turret.rotation;
    }

    private void OnEnable()
    {
        inputActions.Player1.Enable();
        inputActions.Player1.Move.performed += OnMove;
        inputActions.Player1.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player1.Move.canceled -= OnMove;
        inputActions.Player1.Move.performed -= OnMove;
        inputActions.Player1.Disable();
    }

    private void Update()
    {
        if( isAlive )
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
}
