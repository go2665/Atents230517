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
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    // const int i = 10;   // const는 컴파일타임에 값이 결정되고 수정이 불가능하다.
    // readonly int j;     // readonly는 런타임에 값이 결정되고 그 이후로 수정이 불가능하다.

    readonly int isMoveHash = Animator.StringToHash("IsMove");  // "IsMove" 문자열을 숫자로 바꿔서 저장해놓기

    private void Awake()
    {        
        // inputActions = new PlayerInputActions();
        inputActions = new();                   // 객체 생성
        animator = GetComponent<Animator>();    // 이 스크립트가 들어있는 게임 오브젝트에서 Animator 컴포넌트 찾기(없으면 null)
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Player 액션맵을 활성화
        inputActions.Player.Enable();                       

        // Player 액션맵의 Move 액션에 바인딩된 키가 눌러지면 OnMoveInput 실행하도록 연결
        inputActions.Player.Move.performed += OnMoveInput;

        // Player 액션맵의 Move 액션에 바인딩된 키가 때지면 OnMoveInput 실행하도록 연결
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
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
        rotateDir = input.x;
        moveDir = input.y;
        //Debug.Log(input);
        
        animator.SetBool(isMoveHash, !context.canceled);    // 키가 눌러졌는지 떨어졌는지에 따라 애니메이션 변경
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

        // 현재위치 + 초당 moveSpeed 속도로 앞으로 이동 * moveDir(정방향이냐 역방향이냐 정지냐)
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDir * transform.forward);
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
}
