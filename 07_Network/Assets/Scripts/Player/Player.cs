using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 90.0f;

    /// <summary>
    /// 마지막 입력으로 인한 이동 방향과 정도(앞, 뒤, 정지 중 하나)
    /// </summary>
    float moveDir;

    /// <summary>
    /// 마지막 입력으로 인한 회전 방향과 정도(좌회전 or 우회전으로 어느 정도인지)
    /// </summary>
    float rotate;

    /// <summary>
    /// 플레이어의 애니메이션 상태 종류
    /// </summary>
    enum PlayerAnimState
    {
        Idle,
        Walk,
        BackWalk,
        None
    }

    /// <summary>
    /// 플레이어의 현재 애니메이션 상태
    /// </summary>
    PlayerAnimState state = PlayerAnimState.None;

    /// <summary>
    /// 플레이어의 애니메이션 상태를 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    PlayerAnimState State
    {
        get => state;
        set
        {
            if(state != value) // 변경이 일어났을 때만 처리(트리거가 중복으로 쌓이는 현상을 제거하기 위해)
            {
                state = value;                          // 상태 변경하고
                animator.SetTrigger(state.ToString());  // 상태에 따른 트리거 날리기
            }
        }
    }

    // 컴포넌트들
    CharacterController controller;
    Animator animator;
    PlayerInputActions inputActions;
    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MoveForward.performed += OnMoveInput;  // 이동 연결
        inputActions.Player.MoveForward.canceled += OnMoveInput;
        inputActions.Player.Rotate.performed += OnRotateInput;
        inputActions.Player.Rotate.canceled += OnRotateInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Rotate.canceled -= OnRotateInput;
        inputActions.Player.Rotate.performed -= OnRotateInput;
        inputActions.Player.MoveForward.canceled -= OnMoveInput;   // 이동 연결 해제
        inputActions.Player.MoveForward.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        controller.SimpleMove(moveDir * transform.forward);             // 마지막 입력에 따라 이동
        transform.Rotate(0, rotate * Time.deltaTime, 0, Space.World);   // 마지막 입력에 따라 회전
    }

    private void OnMoveInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();   // 입력값 받아오기
        moveDir = moveInput * moveSpeed;                // (앞, 뒤, 정지)와 이동 속도 곱해서 저장하기

        if(moveDir > 0.001f)        // 0.001f는 float 오차때문에 설정한 임계값. 0.001f보다 작으면 0으로 취급하기
        {
            // 전진
            State = PlayerAnimState.Walk;
        }
        else if(moveDir < -0.001f)
        {
            // 후진
            State = PlayerAnimState.BackWalk;
        }
        else
        {
            // 정지
            State = PlayerAnimState.Idle;
        }
    }

    private void OnRotateInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();

        rotate = rotateInput * rotateSpeed;         // (좌회전, 우회전)과 회전 속도 곱해서 저장하기
    }
}
