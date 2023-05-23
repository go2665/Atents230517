using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // public으로 만든 변수는 인스팩터 창에서 수정이 가능하다.
    public float speed = 2.0f;

    Vector3 dir = Vector3.zero;

    PlayerInputAction inputAction;

    // 게임 오브젝트가 생성이 완료되면 호출되는 함수
    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    // 게임 오브젝트를 활성화 할 때 호출되는 함수
    private void OnEnable()
    {
        // started;      // 액션이 일어난 직후(버튼을 눌렀을 때)
        // performed;    // 확실하게 액션이 일어났을 때(버튼을 확실하게 눌렀을 때)
        // canceled;     // 액션이 취소 되었을 때(버튼을 땠을 때)

        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    // Start는 첫번째 프레임 업데이트가 일어나기 전에 호출된다.(시작할 때 호출된다.)
    void Start()
    {
        //Debug.Log("Player의 Start");
        Vector3 newPosition = new Vector3(1, 2, 3); // 새 위치는 (1,2,3)
        //newPosition.x = 3;  // (3,2,3)
        //this.gameObject.transform.position = newPosition; // 아래와 똑같은 코드
        //transform.position = newPosition;           // 위치를 newPosition으로 변경

        transform.position = Vector3.zero;           // 위치를 (0,0,0)으로 변경
    }

    // Update는 매 프레임마다 한번씩 호출된다.(게임 진행 중에 계속 호출된다.)
    void Update()
    {
        //Debug.Log("Player의 Update"); 
        // 플레이어를 계속 오른쪽으로 움직이게 하기
        // Time.deltaTime : 이전 Update함수가 호출되고 이번 Update 함수가 호출될 때까지 걸린 시간
        transform.position += (Time.deltaTime * speed * dir); // 초당 speed의 속도로 dir방향으로 움직이기

        // InputManager : 옛날 방식
        //if( Input.GetKey(KeyCode.Space) ) {}
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log($"이동 입력 - {value}");

        dir = value;
        //dir.x = value.x;
        //dir.y = value.y;
        //dir.z = 0.0f;

        // 1. 입력된 이동 방향으로 이동하기
        // 2. shift키를 누르고 있으면 이동속도가 2배가 되게 만들기
    }

}
