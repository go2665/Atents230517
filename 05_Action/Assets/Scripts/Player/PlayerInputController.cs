using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;

    float currentSpeed = 5.0f;

    enum MoveMode
    {
        Walk = 0,
        Run
    }

    MoveMode moveSpeedMode = MoveMode.Run;
    MoveMode MoveSpeedMode
    {
        get => moveSpeedMode;
        set
        {
        }
    }

    Vector3 inputDir = Vector3.zero;

    PlayerInputActions inputActions;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // 좀 더 수동에 가까운 느낌
        //characterController.SimpleMove(currentSpeed * inputDir);    // 좀 더 자동에 가까운 느낌
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.y = 0.0f;
        inputDir.z = input.y;

        // inputDir.y = -2.0f; // 자동으로 안내려갈 경우 강제로 바닥으로 내리기
    }

    // 1. shift 키를 누르면 이동 모드가 변경된다. (OnMoveModeChange)
    // 2. 이동 속도에 따라 재생되는 애니메이션 변경( MoveBlendTree 조정하기)
    // 3. 카메라 기준으로 입력시 바라보는 방향이 변경됨
    //      3.1. W를 누르면 카메라의 앞쪽 방향으로 이동해야 한다.
    //      3.2. A를 누르면 카메라의 왼쪽 방향으로 이동해야 한다.
    //      3.3. D를 누르면 카메라의 오른쪽 방향으로 이동해야 한다.
    //      3.4. S를 누르면 카메라의 뒤쪽 방향으로 이동해야 한다.



}
