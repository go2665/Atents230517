using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // public으로 만든 변수는 인스팩터 창에서 수정이 가능하다.

    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float speed = 2.0f;

    /// <summary>
    /// 부스트 상태를 나타내는 변수(일반 상황일 때는 1, 부스트 상황일 때는 2)
    /// </summary>
    float boost = 1.0f;

    /// <summary>
    /// 플레이어의 입력된 이동 방향
    /// </summary>
    Vector3 direction = Vector3.zero;   // 초기값으로 방향은 지정이 안되어있다.

    /// <summary>
    /// 애니메이터 컨트롤러 컴포넌트
    /// </summary>
    Animator anim;

    readonly int InputY_String = Animator.StringToHash("InputY");

    /// <summary>
    /// 입력 액션 에셋
    /// </summary>
    PlayerInputAction inputAction;

    // 게임 오브젝트가 생성이 완료되면 호출되는 함수
    private void Awake()
    {
        Debug.Log("Player의 Awake");
        inputAction = new PlayerInputAction();  // 입력 액션 객체 만들기

        anim = GetComponent<Animator>();        // Animator 컴포넌트를 찾아서 리턴하는 함수. 없으면 null
    }

    // 게임 오브젝트를 활성화 할 때 호출되는 함수
    private void OnEnable()
    {
        // started;      // 액션이 일어난 직후(버튼을 눌렀을 때)
        // performed;    // 확실하게 액션이 일어났을 때(버튼을 확실하게 눌렀을 때)
        // canceled;     // 액션이 취소 되었을 때(버튼을 땠을 때)

        Debug.Log("Player의 활성화");
        inputAction.Player.Enable();                        // 입력 활성화
        inputAction.Player.Move.performed += OnMove;        // 액션별 함수 연결
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Boost.performed += OnBoost;
        inputAction.Player.Boost.canceled += OnBoost;
    }

    // 게임 오브젝트가 비활성화 될 때 호출되는 함수
    private void OnDisable()
    {
        Debug.Log("Player의 비활성화");
        inputAction.Player.Boost.canceled -= OnBoost;
        inputAction.Player.Boost.performed -= OnBoost;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    // Start는 첫번째 프레임 업데이트가 일어나기 전에 호출된다.(시작할 때 호출된다.)
    void Start()
    {
        Debug.Log("Player의 Start");
        //Vector3 newPosition = new Vector3(1, 2, 3); // 새 위치는 (1,2,3)
        //newPosition.x = 3;  // (3,2,3)
        //this.gameObject.transform.position = newPosition; // 아래와 똑같은 코드
        //transform.position = newPosition;           // 위치를 newPosition으로 변경

        //transform.position = Vector3.zero;           // 위치를 (0,0,0)으로 변경
    }

    // Update는 매 프레임마다 한번씩 호출된다.(게임 진행 중에 계속 호출된다.)
    void Update()
    {
        //Debug.Log("Player의 Update"); 
        // 플레이어를 계속 오른쪽으로 움직이게 하기
        // Time.deltaTime : 이전 Update함수가 호출되고 이번 Update 함수가 호출될 때까지 걸린 시간

        // 초당 speed의 속도(부스트인 상황은 2배)로 dir방향으로 움직이기
        //transform.position += (Time.deltaTime * speed * boost * direction);   // 아래와 같은 코드
        transform.Translate(Time.deltaTime * speed * boost * direction);

        // InputManager : 옛날 방식
        //if( Input.GetKey(KeyCode.Space) ) {}
    }

    // WASD 입력이 있을 때 실행되는 함수
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();   // 입력 값 받아오기(Action type이 value인 경우만 가능)
        Debug.Log($"이동 입력 - {value}");

        direction = value;      // 입력 받은 값을 맴버변수인 direction에 저장하기
        
        //anim.SetFloat("InputY", direction.y);     // 아래 코드와 같은 결과
        anim.SetFloat(InputY_String, direction.y);

        //dir.x = value.x;
        //dir.y = value.y;
        //dir.z = 0.0f;
    }

    // 쉬프트 키가 눌러졌을 때 실행되는 함수
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // 버튼이 떨어져서 실행된 경우
            boost = 1.0f;
        }
        else
        {
            // 버튼이 눌러져서 실행된 경우
            boost = 2.0f;
        }
    }

}
