using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 현재 이동 속도(걷기 or 달리기)
    /// </summary>
    float currentSpeed = 5.0f;

    /// <summary>
    /// 이동 상태용 enum
    /// </summary>
    enum MoveMode
    {
        Walk = 0,
        Run
    }

    /// <summary>
    /// 현재 이동 상태
    /// </summary>
    MoveMode moveSpeedMode = MoveMode.Run;

    /// <summary>
    /// 현재 이동 상태 설정 및 확인용 프로퍼티
    /// </summary>
    MoveMode MoveSpeedMode
    {
        get => moveSpeedMode;
        set
        {
            moveSpeedMode = value;  // 받은 대로 설정
            switch (moveSpeedMode)
            {
                case MoveMode.Walk:
                    currentSpeed = walkSpeed;       // 현재 속도 변경
                    if (inputDir != Vector3.zero)   
                    {
                        animator.SetFloat(Speed_Hash, AnimatorWalkSpeed);   // 이동 중일 때만 animator의 speed도 변경
                    }
                    break;
                case MoveMode.Run:
                    currentSpeed = runSpeed;
                    if (inputDir != Vector3.zero)
                    {
                        animator.SetFloat(Speed_Hash, AnimatorRunSpeed);
                    }
                    break;
            }
        }
    }

    const float AnimatorStopSpeed = 0.0f;   // 정지 상태일 때 animator의 speed 값
    const float AnimatorWalkSpeed = 0.3f;   // 걷는 상태일 때 animator의 speed 값
    const float AnimatorRunSpeed = 1.0f;    // 달리는 상태일 때 animator의 speed 값

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 최종적으로 바라보아야 할 방향
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 인풋 액션 객체
    /// </summary>
    PlayerInputActions inputActions;

    CharacterController characterController;
    Animator animator;

   /// 애니메이터 파라메터의 해시
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        MoveSpeedMode = MoveMode.Run;           // 시작 속도 설정
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // 좀 더 수동에 가까운 느낌
        //characterController.SimpleMove(currentSpeed * inputDir);    // 좀 더 자동에 가까운 느낌

        // targetRotation까지 초당 1/turnSpeed 속도로 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();   // 현재 입력 저장
        inputDir.x = input.x;
        inputDir.y = 0.0f;
        inputDir.z = input.y;

        if( !context.canceled )
        {
            // Move 액션에 바인딩된 키가 눌려졌을 때
            Quaternion camYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y 회전만 추출
            inputDir = camYRotation * inputDir;     // 카메라 Y회전을 입력방향에 곱해서 입력방향을 회전시키기
            targetRotation = Quaternion.LookRotation(inputDir); // inputDir기준으로 바라보는 회전 만들기

            switch ( MoveSpeedMode)    // 현재 이동 모드에 따라 애니메이터의 Speed 변경
            {
                case MoveMode.Walk:
                    currentSpeed = walkSpeed;
                    animator.SetFloat(Speed_Hash, AnimatorWalkSpeed);
                    break;
                case MoveMode.Run:
                    currentSpeed = runSpeed;
                    animator.SetFloat(Speed_Hash, AnimatorRunSpeed);
                    break;
            }
        }
        else
        {
            // Move 액션에 바인딩된 키가 떨어졌을 때
            currentSpeed = 0.0f;
            animator.SetFloat(Speed_Hash, AnimatorStopSpeed);   // 애니메이터의 Speed를 0으로 설정
        }

        // inputDir.y = -2.0f; // 자동으로 안내려갈 경우 강제로 바닥으로 내리기
    }

    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        // Shift키가 눌려졌을 때
        if(MoveSpeedMode == MoveMode.Walk)
        {
            MoveSpeedMode = MoveMode.Run;   // walk면 run으로
        }
        else
        {
            MoveSpeedMode = MoveMode.Walk;  // run이면 walk로
        }        
    }

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if( moveSpeedMode == MoveMode.Walk || currentSpeed < 0.001f)    // 걷는 상태이거나 정지 상태일 때만 공격 가능
        {
            animator.SetTrigger(Attack_Hash);
        }
    }

}
