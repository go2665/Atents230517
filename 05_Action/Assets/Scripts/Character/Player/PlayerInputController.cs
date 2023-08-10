using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    /// 현재 이동 속도(걷기 or 달리기). 움직이는 입력이 들어오면 walk나 run 속도, 정지하면 0
    /// </summary>
    float currentSpeed = 0.0f;

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
                    if (currentSpeed > 0)   // 이동 중에 이동 상태가 변경되면 속도 바로 적용하고, 정지상태일 때는 계속 0으로 있게 조건 추가
                    {
                        currentSpeed = walkSpeed;       // 현재 속도 변경
                    }
                    if (inputDir != Vector3.zero)   
                    {
                        animator.SetFloat(Speed_Hash, AnimatorWalkSpeed);   // 이동 중일 때만 animator의 speed도 변경
                    }
                    break;
                case MoveMode.Run:
                    if(currentSpeed > 0)   // 이동 중에 이동 상태가 변경되면 속도 바로 적용하고, 정지상태일 때는 계속 0으로 있게 조건 추가
                    {
                        currentSpeed = runSpeed;
                    }
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

    /// <summary>
    /// 아이템 줍기 버튼이 눌려졌다는 신호를 보내는 델리게이트
    /// </summary>
    public System.Action onItemPickup;

    /// <summary>
    /// 락온 버튼이 눌려졌다는 신호를 보내는 델리게이트
    /// </summary>
    public System.Action onLockOn;

    /// <summary>
    /// 스킬을 시작했다는 신호를 보내는 델리게이트
    /// </summary>
    public Action onSkillStart;

    /// <summary>
    /// 스킬을 끝냈다는 신호를 보내는 델리게이트
    /// </summary>
    public Action onSkillEnd;

    /// <summary>
    /// 이 컨트롤러로 조정하는 플레이어
    /// </summary>
    Player player;

    CharacterController characterController;
    Animator animator;

   /// 애니메이터 파라메터의 해시
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int SkillStart_Hash = Animator.StringToHash("SkillStart");
    readonly int SkillEnd_Hash = Animator.StringToHash("SkillEnd");

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();

        player = GetComponent<Player>();
        player.onDie += inputActions.Player.Disable;    // 플레이어가 죽으면 입력안되게 만들기
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.Skill.performed += OnSkillStart;
        inputActions.Player.Skill.canceled += OnSkillEnd;
        inputActions.Player.PickUp.performed += OnPickUp;
        inputActions.Player.LockOn.performed += OnLockOn;
    }

    private void OnDisable()
    {
        inputActions.Player.LockOn.performed -= OnLockOn;
        inputActions.Player.PickUp.performed -= OnPickUp;
        inputActions.Player.Skill.canceled -= OnSkillEnd;
        inputActions.Player.Skill.performed -= OnSkillStart;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        MoveSpeedMode = MoveMode.Run;           // 시작 속도 설정

        InventoryUI invenUI = GameManager.Inst.InvenUI;
        if(invenUI != null)
        {
            invenUI.onInventoryOpen += inputActions.Player.Attack.Disable;  // 인벤토리가 열릴때는 공격 못함
            invenUI.onInventoryClose += inputActions.Player.Attack.Enable;  // 인벤토리가 닫히면 공격 가능
        }
    }

    private void Update()
    {
        if( player.IsAlive )
        {
            characterController.Move(Time.deltaTime * currentSpeed * inputDir); // 좀 더 수동에 가까운 느낌
            //characterController.SimpleMove(currentSpeed * inputDir);    // 좀 더 자동에 가까운 느낌
                        
            if( player.LockOnTarget != null)   // 락온한 대상이 있으면
            {
                // targetRotation을 락온한 대상을 바라보는 회전으로 변경
                targetRotation = Quaternion.LookRotation(player.LockOnTarget.position - player.transform.position);
            }

            // targetRotation까지 초당 1/turnSpeed 속도로 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
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

    private void OnSkillStart(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        animator.SetTrigger(SkillStart_Hash);
        animator.SetBool(SkillEnd_Hash, false);
        onSkillStart?.Invoke();
    }

    private void OnSkillEnd(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        SkillEndSequence();
    }

    private void OnPickUp(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onItemPickup?.Invoke();
    }

    private void OnLockOn(InputAction.CallbackContext _)
    {
        onLockOn?.Invoke();
    }

    /// <summary>
    /// 스킬이 끝났을 때 처리를 모아놓은 함수
    /// </summary>
    public void SkillEndSequence()
    {
        animator.SetBool(SkillEnd_Hash, true);  // 애니메이션 정지 시키기
        onSkillEnd?.Invoke();                   // 알람 보내기
    }
}
