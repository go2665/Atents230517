using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Collections;
using Unity.VisualScripting;

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

    /// <summary>
    /// 지금 내가 보낸 채팅
    /// </summary>
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();

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

    NetworkVariable<PlayerAnimState> netAnimState = new NetworkVariable<PlayerAnimState>();

    /// <summary>
    /// 플레이어의 애니메이션 상태를 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    //PlayerAnimState State
    //{
    //    get => state;
    //    set
    //    {
    //        if (state != value) // 변경이 일어났을 때만 처리(트리거가 중복으로 쌓이는 현상을 제거하기 위해)
    //        {
    //            state = value;                          // 상태 변경하고
    //            animator.SetTrigger(state.ToString());  // 상태에 따른 트리거 날리기
    //        }
    //    }
    //}

    NetworkVariable<bool> netEffectState = new NetworkVariable<bool>(false);
    public bool IsEffectOn
    {
        get => netEffectState.Value;
        set 
        {
            if(netEffectState.Value != value)
            {
                if(IsServer)
                {
                    netEffectState.Value = value;
                }
                else
                {
                    UpdateEffectStateServerRpc(value);
                }
            }
        }
    }

    /// <summary>
    /// 총알용 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 오브용 프리팹
    /// </summary>
    public GameObject orbPrefab;

    /// <summary>
    /// 총알이나 오브를 발사할 트랜스폼
    /// </summary>
    Transform firePosition;

    /// 컴포넌트
    CharacterController controller;
    Animator animator;
    Material bodyMaterial;

    /// 인풋 액션
    PlayerInputActions inputActions;

    // 초기 실행 함수 순서
    // Awake -> OnEnable -> OnNetworkSpawn -> Start

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        position.OnValueChanged += OnPositionChange;    // 위치 값이 변경되었을 때 실행할 함수 등록

        chatString.OnValueChanged += OnChatRecieve;     // 채팅이 입력되면 실행될 함수 등록

        netAnimState.OnValueChanged += OnAnimStateChange;
        netEffectState.OnValueChanged += OnEffectStateChange;

        Renderer meshRenderer = GetComponentInChildren<Renderer>();
        bodyMaterial = meshRenderer.material;

        firePosition = transform.GetChild(4);           // 발사 위치 가져오기
    }

    private void Update()
    {
        if(netMoveDir.Value != 0.0f)    // 값이 변경 되었을 때만 실행(안그러면 transform의 position을 수동으로 변경해도 적용이 안된다.)
        { 
            controller.SimpleMove(netMoveDir.Value * transform.forward);            // 마지막 입력에 따라 이동
        }
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
            inputActions.Player.Attack01.performed += OnAttack01;       // 공격 연결
            inputActions.Player.Attack02.performed += OnAttack02;

            SetSpawnPosition();     // 스폰될 위치 결정

            GameManager.Inst.VCam.Follow = transform.GetChild(0);       // 카메라 붙이기            
            GameManager.Inst.VirtualPad.onMoveInput = (inputDir) =>
            {
                SetMoveInput(inputDir.y);
                SetRotateInput(inputDir.x);
                Debug.Log(inputDir);
            };

            GameManager.Inst.VirtualPad.onAttack01Input = Attack01;     // 가상패드의 버튼과 공격함수 연결
            GameManager.Inst.VirtualPad.onAttack02Input = Attack02;
        }
    }

    /// <summary>
    /// 이 네트워크용 게임 오브젝트가 네트워크 상에서 디스폰되었을 때 실행되는 함수
    /// </summary>
    public override void OnNetworkDespawn()
    {
        if( IsOwner && inputActions != null )   // 오너이고 인풋액션을 만들었으면 연결 끊고 제거
        {
            if(GameManager.Inst != null && GameManager.Inst.VirtualPad != null)
                GameManager.Inst.VirtualPad.onMoveInput = null;

            inputActions.Player.Attack02.performed -= OnAttack02;       // 공격 연결 해제
            inputActions.Player.Attack01.performed -= OnAttack01;
            inputActions.Player.Rotate.canceled -= OnRotateInput;       // 회전 연결 해제
            inputActions.Player.Rotate.performed -= OnRotateInput;
            inputActions.Player.MoveForward.canceled -= OnMoveInput;    // 이동 연결 해제
            inputActions.Player.MoveForward.performed -= OnMoveInput;
            inputActions.Player.Disable();
            inputActions = null;
        }
    }

    public void Die()
    {
        if(IsOwner)
        {
            //transform.position = GameManager.Inst.GetPlayerRespawnPosition();
            if (NetworkManager.Singleton.IsServer)  // 죽었을 때 netMoveDir을 0으로 만들어서 멈추게 만들기
            {
                netMoveDir.Value = 0.0f; 
            }
            else
            {
                MoveRequestServerRpc(0.0f);
            }
            SetSpawnPosition();                     // 위치 새로 설정

            transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);    // 회전도 랜덤으로 설정
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();   // 입력값 받아오기
        SetMoveInput(moveInput);
    }

    private void SetMoveInput(float moveInput)
    {
        float moveDir = moveInput * moveSpeed;          // (앞, 뒤, 정지)와 이동 속도 곱해서 이동 정도 계산

        if (NetworkManager.Singleton.IsServer)
        {
            netMoveDir.Value = moveDir;                 // 서버이면 네트워크 변수 직접 수정
        }
        else
        {
            MoveRequestServerRpc(moveDir);              // 클라이언트면 Rpc를 통해 서버에 수정 요청
        }

        if (moveDir > 0.001f)        // 0.001f는 float 오차때문에 설정한 임계값. 0.001f보다 작으면 0으로 취급하기
        {
            // 전진
            state = PlayerAnimState.Walk;
        }
        else if (moveDir < -0.001f)
        {
            // 후진
            state = PlayerAnimState.BackWalk;
        }
        else
        {
            // 정지
            state = PlayerAnimState.Idle;
        }

        // 상태가 변경되면 네트워크 변수도 변경
        if (state != netAnimState.Value)
        {
            if (IsServer)
            {
                netAnimState.Value = state;
            }
            else
            {
                UpdateAnimStateServerRpc(state);
            }
        }
    }

    private void OnRotateInput(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();
        SetRotateInput(rotateInput);
    }

    private void SetRotateInput(float rotateInput)
    {
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

    private void OnAttack01(InputAction.CallbackContext context)
    {
        //GameManager.Inst.Log($"{GameManager.Inst.UserName} : {"왼쪽클릭"}");
        Attack01();
    }

    private void OnAttack02(InputAction.CallbackContext context)
    {
        //GameManager.Inst.Log($"{GameManager.Inst.UserName} : {"오른쪽클릭"}");
        Attack02();
    }

    /// <summary>
    /// 총알을 발사하는 함수
    /// </summary>
    public void Attack01()
    {
        RequestSpanwBulletServerRpc();  // 서버RPC로 총알 생성 요청
    }

    /// <summary>
    /// 오브를 발사하는 함수
    /// </summary>
    public void Attack02()
    {
        RequestEnergyOrbServerRpc();    // 서버RPC로 오브 생성 요청
    }


    /// <summary>
    /// 이 네트워크 오브젝트가 스폰될 위치를 결정하는 함수
    /// </summary>
    void SetSpawnPosition()
    {
        //Vector3 newPos = Random.insideUnitSphere;   // (-1,-1,-1) ~ (1,1,1) 사이를 랜덤으로 결정
        //newPos.y = 0;                               // y는 0으로 설정

        Vector3 newPos = GameManager.Inst.GetPlayerRespawnPosition();   // 리스폰 위치는 게임메니저에서 받아오기
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

    /// <summary>
    /// 채팅을 전송하는 함수
    /// </summary>
    /// <param name="message">채팅으로 보낼 메세지</param>
    public void SendChat(string message)
    {
        if (IsServer)
        {
            chatString.Value = message;     // 내가 서버면 직접 수정
        }
        else
        {
            RequestChatServerRpc(message);  // 내가 서버가 아니면 서버에게 요청
        }
    }

    /// <summary>
    /// chatString이 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="previousValue">이전값</param>
    /// <param name="newValue">현재값</param>
    private void OnChatRecieve(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        GameManager.Inst.Log(newValue.ToString());  // 변경되면 로거로 찍기
    }

    /// <summary>
    /// netAnimState가 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void OnAnimStateChange(PlayerAnimState previousValue, PlayerAnimState newValue)
    {
        animator.SetTrigger(newValue.ToString());
    }

    private void OnEffectStateChange(bool previousValue, bool newValue)
    {
        if(newValue)
        {
            bodyMaterial.SetFloat("_EmissionValue", 1);
        }
        else
        {
            bodyMaterial.SetFloat("_EmissionValue", 0);
        }
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

    [ServerRpc]
    void RequestChatServerRpc(string text)
    {
        chatString.Value = text;
    }

    [ServerRpc]
    void UpdateAnimStateServerRpc(PlayerAnimState newState)
    {
        netAnimState.Value = newState;
    }

    [ServerRpc]
    void UpdateEffectStateServerRpc(bool isEffectOn)
    {
        netEffectState.Value = isEffectOn;
    }

    [ServerRpc]
    void RequestSpanwBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab);                  // 프리팹 생성하고
        bullet.transform.position = firePosition.position;              // 위치랑 회전을 설정하고
        bullet.transform.rotation = firePosition.rotation;
        NetworkObject netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn(true);                                             // 스폰 요청하기
    }

    [ServerRpc]
    void RequestEnergyOrbServerRpc()
    {
        GameObject energyOrb = Instantiate(orbPrefab);
        energyOrb.transform.position = firePosition.position;
        energyOrb.transform.rotation = firePosition.rotation;
        NetworkObject netObj = energyOrb.GetComponent<NetworkObject>();
        netObj.Spawn(true);
    }
}
