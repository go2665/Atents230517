using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // PlayerInputActions 클래스는 유니티 에디터를 통해서 자동 생성
    PlayerInputActions inputActions;        // 플레이어 입력 처리용 클래스

    Animator animator;
    Rigidbody rigid;

    /// <summary>
    /// 이동 방향 -1(후진) ~ 1(전진)
    /// </summary>
    float moveDir = 0.0f;

    /// <summary>
    /// 입력받은 회전 방향 -1(좌회전) ~ 1(우회전)
    /// </summary>
    float rotateDir = 0.0f;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    float currentMoveSpeed = 5.0f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpPower = 6.0f;

    /// <summary>
    /// 점프 중인지 표시(true면 점프중)
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    public float jumpCooltimeMax = 5.0f;

    /// <summary>
    /// 남아있는 쿨타임 시간
    /// </summary>
    //[SerializeField] : private 맴버도 인스팩터 창에서 볼 수 있다.
    float jumpCooltime = 0.0f;

    private float JumpCoolTime
    {
        get => jumpCooltime;
        set
        {
            jumpCooltime = value;
            if( jumpCooltime < 0.0f ) 
            {
                jumpCooltime = 0.0f;
            }
            onJumpCoolTimeChange?.Invoke(jumpCooltime / jumpCooltimeMax);
        }
    }

    /// <summary>
    /// 쿨타임이 다됬는지 확인하는 프로퍼티
    /// </summary>
    bool IsJumpCoolEnd => (jumpCooltime <= 0.0f);

    /// <summary>
    /// 쿨타임에 변화가 있으면 실행되는 델리게이트
    /// </summary>
    Action<float> onJumpCoolTimeChange;

    // const int i = 10;   // const는 컴파일타임에 값이 결정되고 수정이 불가능하다.
    // readonly int j;     // readonly는 런타임에 값이 결정되고 그 이후로 수정이 불가능하다.

    readonly int isMoveHash = Animator.StringToHash("IsMove");  // "IsMove" 문자열을 숫자로 바꿔서 저장해놓기

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float lifeTimeMax = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime = 10.0f;

    /// <summary>
    /// 수명 확인 및 설정용 프로퍼티
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;
        private set
        {
            if (!isClear)
            {
                lifeTime = value;
                if (lifeTime <= 0.0f)
                {
                    lifeTime = 0.0f;
                    Die();      // 수명이 0보다 작거나 같으면 사망
                }
                onLifeTimeChange?.Invoke(lifeTime / lifeTimeMax);   // 변경이 있을 때마다 신호를 보낸다.
            }
        }
    }

    /// <summary>
    /// 클리어 여부 표현
    /// </summary>
    bool isClear = false;

    /// <summary>
    /// 수명 변화가 있을 때 실행되는 델리게이트
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// 플레이어가 살아있는지 여부
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 죽었다고 알리는 델리게이트. 파라메터는 죽은 당사자.
    /// </summary>
    public Action<Player> onDie;

    private void Awake()
    {        
        // inputActions = new PlayerInputActions();
        inputActions = new();                   // 객체 생성
        animator = GetComponent<Animator>();    // 이 스크립트가 들어있는 게임 오브젝트에서 Animator 컴포넌트 찾기(없으면 null)
        rigid = GetComponent<Rigidbody>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += UseItem;
    }

    private void OnEnable()
    {
        // Player 액션맵을 활성화
        inputActions.Player.Enable();                       

        // Player 액션맵의 Move 액션에 바인딩된 키가 눌러지면 OnMoveInput 실행하도록 연결
        inputActions.Player.Move.performed += OnMoveInput;

        // Player 액션맵의 Move 액션에 바인딩된 키가 때지면 OnMoveInput 실행하도록 연결
        inputActions.Player.Move.canceled += OnMoveInput;

        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Use.performed += OnUseInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Jump.performed -= OnJumpInput;

        // Player 액션맵의 Move 액션에 바인딩 된 키가 때실 때 실행되던 OnMoveInput을 연결 해제
        inputActions.Player.Move.canceled -= OnMoveInput;

        // Player 액션맵의 Move 액션에 바인딩 된 키가 눌러질 때 실행되던 OnMoveInput을 연결 해제
        inputActions.Player.Move.performed -= OnMoveInput;

        // Player 액션맵을 비활성화
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Player 액션맵의 Move 액션에 바인딩 된 키가 눌러질 때 실행되는 함수
    /// </summary>
    /// <param name="context">입력정보</param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();       // 어떻게 입력이 들어왔는지 받기
        SetInput(input, !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        Jump();
    }

    private void OnUseInput(InputAction.CallbackContext _)
    {
        animator.SetTrigger("Use");
    }

    private void Start()
    {
        LifeTime = lifeTimeMax;
        currentMoveSpeed = moveSpeed;

        // 가상 스틱 UI 찾아서 플레이어와 연결하기
        VirtualStick stick = FindObjectOfType<VirtualStick>();
        if (stick != null)
        {
            stick.onMoveInput += (input) => SetInput(input, input != Vector2.zero);            
            //stick.onMoveInput += (input) => SetInput(input, input.sqrMagnitude < 0.01f);            
        }

        // 가상 버튼 UI 찾아서 플레이어와 연결하기
        VirtualButton button = FindObjectOfType<VirtualButton>();
        if(button != null)
        {
            button.onClick += Jump;
            onJumpCoolTimeChange += button.RefreshCoolTime;
        }

        Goal goal = FindObjectOfType<Goal>();
        if (goal != null)
        {
            goal.onGoalIn += () => isClear = true;
        }
    }

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        JumpCoolTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        // w가 눌려져 있으면 플레이어의 앞방향으로 이동한다.
        // s가 눌려져 있으면 플레이어의 뒷방향으로 이동한다.

        if( moveDir > 0.01f || moveDir < -0.01f)
        {
            // 현재위치 + 초당 currentMoveSpeed 속도로 앞으로 이동 * moveDir(정방향이냐 역방향이냐 정지냐)
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDir * transform.forward);
        }    
    }

    void Rotate()
    {
        // a가 눌려져 있으면 좌회전한다.
        // d가 눌려져 있으면 우회전한다.

        // Quaternion.Euler() : x,y,z축으로 얼마만큼 회전 시킬 것인지를 파라메터로 받음
        // Quaternion.AngleAxis : 특정 축을 기준으로 몇도만큼 회전 시킬 것인지를 파라메터로 받음
        // Quaternion.FromToRotation : 시작 방향 벡터가 목표 방향 벡터로 되는 회전을 만들어주는 함수
        // Quaternion.Lerp : 시작 회전에서 목표 회전으로 보간하는 함수
        // Quaternion.Slerp : 시작 회전에서 목표 회전으로 보간하는 함수(곡선으로 보간)
        // Quaternion.LookRotation : 특정 방향을 바라보는 회전을 만들어주는 함수
        
        // 변화할 회전 구하기(약간 움직이는 것)
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDir,
            transform.up);

        // 현재 회전에 변화할 회전을 곱해서 현재 각도에서 rotate만큼 추가로 회전한 결과 만들기
        rigid.MoveRotation(rigid.rotation * rotate);    
    }

    /// <summary>
    /// 점프하는 함수
    /// </summary>
    void Jump()
    {
        if(!isJumping && IsJumpCoolEnd) // 점프중이 아니고 점프 쿨타임이 다 되었을 때만 점프
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);  // 위로 힘을 가하고
            JumpCoolTime = jumpCooltimeMax; // 쿨타임 초기화
            isJumping = true;               // 점프 중이라고 표시
        }
    }

    /// <summary>
    /// 상호작용 가능한 오브젝트를 사용하는 함수
    /// </summary>
    /// <param name="interactable">사용할 대상</param>
    private void UseItem(IInteractable interactable)
    {
        if (interactable.IsDirectUse)
        {
            interactable.Use();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;  // 바닥에 닿으면 다시 점프 가능
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove = OnRideMovingObject;   // 플렛폼 베이스에 탑승하면 함수연결
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove = null;             // 플렛폼베이스와 떨어졌을 때 연결 해제
        }
    }

    /// <summary>
    /// 움직이는 물체에 탑승했을 때 움직이는 물체가 보내는 델리게이트를 처리하는 함수
    /// </summary>
    /// <param name="delta">이번에 움직인 정도</param>
    private void OnRideMovingObject(Vector3 delta)
    {
        rigid.MovePosition(rigid.position + delta); // delta만큼 추가로 움직임
    }

    /// <summary>
    /// 죽는 처리용 함수
    /// </summary>
    public void Die()
    {
        if( isAlive )
        {            
            animator.SetTrigger("Die");                     // 죽는 애니메이션 재생
            inputActions.Player.Disable();                  // 입력 처리 중지

            rigid.constraints = RigidbodyConstraints.None;  // 물리 잠금 전부 풀기

            // 뒤로 넘어트리기
            Transform head = transform.GetChild(0);
            rigid.AddForceAtPosition(0.5f * (-transform.forward), head.position, ForceMode.Impulse);
            // 떼굴떼굴 돌기
            rigid.AddTorque(transform.up * 1.2f, ForceMode.Impulse);

            isAlive = false;

            onDie?.Invoke(this);    // 자신이 죽었음을 알림
        }
    }

    /// <summary>
    /// 강제로 점프 중이라고 설정하는 함수
    /// </summary>
    public void SetForceJumpMode()
    {
        isJumping = true;
    }

    /// <summary>
    /// 속도 디버프용 함수
    /// </summary>
    /// <param name="ratio">원본대비 새로 적용될 비율</param>
    public void SetSpeedDebuffe(float ratio)
    {
        currentMoveSpeed = moveSpeed * ratio;
    }

    /// <summary>
    /// 원래 속도로 복구하는 함수
    /// </summary>
    public void RestoreMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }

    private void SetInput(Vector2 input, bool isMove)
    {
        rotateDir = input.x;
        moveDir = input.y;

        animator.SetBool(isMoveHash, isMove);    // 키가 눌러졌는지 떨어졌는지에 따라 애니메이션 변경
    }
}
