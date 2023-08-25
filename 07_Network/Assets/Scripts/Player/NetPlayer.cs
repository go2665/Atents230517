using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class NetPlayer : NetworkBehaviour
{
    /// <summary>
    /// 플레이어의 위치를 결정할 네트워크변수
    /// </summary>
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 플레이어의 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 입력받은 전진/후진 정도
    /// </summary>
    NetworkVariable<float> netMoveDir = new NetworkVariable<float>();

    /// <summary>
    /// 입력받은 회전 정도
    /// </summary>
    NetworkVariable<float> netRotateDir = new NetworkVariable<float>();

    /// 컴포넌트
    CharacterController controller;

    /// 인풋 액션
    PlayerInputActions inputActions;

    // 초기 실행 함수 순서
    // Awake -> OnEnable -> OnNetworkSpawn -> Start

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        position.OnValueChanged += OnPositionChange;    // 위치 값이 변경되었을 때 실행할 함수 등록
    }

    private void Update()
    {
        controller.SimpleMove(netMoveDir.Value * transform.forward);                // 마지막 입력에 따라 이동
        transform.Rotate(0, netRotateDir.Value * Time.deltaTime, 0, Space.World);   // 마지막 입력에 따라 회전
    }

    /// <summary>
    /// 이 네트워크용 게임오브젝트가 네트워크 상에 스폰되었을 때 실행되는 함수
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if( IsOwner )   // 오너일때만
        {
            inputActions = new PlayerInputActions();                    // 인풋액션 만들고

            inputActions.Player.Enable();
            inputActions.Player.MoveForward.performed += OnMoveInput;   // 이동 연결
            inputActions.Player.MoveForward.canceled += OnMoveInput;
            inputActions.Player.Rotate.performed += OnRotateInput;      // 회전 연결
            inputActions.Player.Rotate.canceled += OnRotateInput;

            SetSpawnPosition();     // 스폰될 위치 결정
        }
    }

    /// <summary>
    /// 이 네트워크용 게임 오브젝트가 네트워크 상에서 디스폰되었을 때 실행되는 함수
    /// </summary>
    public override void OnNetworkDespawn()
    {
        if( IsOwner && inputActions != null )   // 오너이고 인풋액션을 만들었으면 연결 끊고 제거
        {
            inputActions.Player.Rotate.canceled -= OnRotateInput;       // 회전 연결 해제
            inputActions.Player.Rotate.performed -= OnRotateInput;
            inputActions.Player.MoveForward.canceled -= OnMoveInput;    // 이동 연결 해제
            inputActions.Player.MoveForward.performed -= OnMoveInput;
            inputActions.Player.Disable();
            inputActions = null;
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();   // 입력값 받아오기
        float moveDir = moveInput * moveSpeed;          // (앞, 뒤, 정지)와 이동 속도 곱해서 이동 정도 계산

        if(NetworkManager.Singleton.IsServer)
        {
            netMoveDir.Value = moveDir;                 // 서버이면 네트워크 변수 직접 수정
        }
        else
        {
            MoveRequestServerRpc(moveDir);              // 클라이언트면 Rpc를 통해 서버에 수정 요청
        }
    }

    private void OnRotateInput(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();

        float rotateDir = rotateInput * rotateSpeed;    // (좌회전, 우회전)과 회전 속도 곱해서 회전 정도 계산
        if (NetworkManager.Singleton.IsServer)
        {
            netRotateDir.Value = rotateDir;             // 서버이면 네트워크 변수 직접 수정
        }
        else
        {
            RotateRequestServerRpc(rotateDir);          // 클라이언트면 Rpc를 통해 서버에 수정 요청
        }
    }

    /// <summary>
    /// 이 네트워크 오브젝트가 스폰될 위치를 결정하는 함수
    /// </summary>
    void SetSpawnPosition()
    {
        Vector3 newPos = Random.insideUnitSphere;   // (-1,-1,-1) ~ (1,1,1) 사이를 랜덤으로 결정
        newPos.y = 0;                               // y는 0으로 설정
        if (NetworkManager.Singleton.IsServer)
        {
            position.Value = newPos;                // 서버이면 네트워크 변수 직접 수정
        }
        else
        {
            SubmitPositionRequestServerRpc(newPos); // 클라이언트면 Rpc를 통해 서버에 수정 요청
        }
    }

    /// <summary>
    /// 네트워크 변수 position이 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="previousValue">position이 원래 가지고 있던 값</param>
    /// <param name="newValue">position이 새로 가지게 된 값</param>
    private void OnPositionChange(Vector3 previousValue, Vector3 newValue)
    {
        transform.position = newValue;
    }

    // ServerRpc는 서버에서 특정함수를 실행하는 것

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 newPos)
    {
        position.Value = newPos;
    }

    [ServerRpc]
    void MoveRequestServerRpc(float move)
    {
        netMoveDir.Value = move;
    }

    [ServerRpc]
    void RotateRequestServerRpc(float rotate)
    {
        netRotateDir.Value = rotate;
    }

}
