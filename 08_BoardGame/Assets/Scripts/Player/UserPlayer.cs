using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public class UserPlayer : PlayerBase
{
    /// <summary>
    /// 지금 배치하려는 배
    /// </summary>
    Ship selectedShip;

    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            if (selectedShip != value)
            {
                if (selectedShip != null)
                {
                    selectedShip.SetMaterialType();
                    if (!selectedShip.IsDeployed)
                    {
                        selectedShip.gameObject.SetActive(false);
                    }
                }
            }
            selectedShip = value;

            if (selectedShip != null)
            {
                selectedShip.SetMaterialType(false);

                Vector2 screen = Mouse.current.position.ReadValue();
                Vector3 world = Camera.main.ScreenToWorldPoint(screen);
                world.y = board.transform.position.y;
                selectedShip.transform.position = world;
                selectedShip.gameObject.SetActive(true);
            }
        }
    }

    private GameState state = GameState.Title;

    // 입력 관련 델리게이트 -------------------------------------------------------------------------
    // 상태별로 따로 처리(만약 null이면 그 상태에서 수행하는 일이 없다는 의미)
    Action<Vector2>[] onMouseClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    protected override void Awake()
    {
        base.Awake();

        int length = Enum.GetValues(typeof(GameState)).Length;
        onMouseClick = new Action<Vector2>[length];
        onMouseMove = new Action<Vector2>[length];
        onMouseWheel = new Action<float>[length];

        onMouseClick[(int)GameState.ShipDeployment] = OnClick_ShipDeployment;
        onMouseClick[(int)GameState.Battle] = OnClick_Battle;
        onMouseMove[(int)GameState.ShipDeployment] = OnMouseMove_ShipDeployment;
        onMouseWheel[(int)GameState.ShipDeployment] = OnMouseWheel_ShipDeployment;
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Inst.onStateChange += OnStateChange;    // 반드시 GameManager.OnInitialize 이후에 실행되어야 한다.

        GameManager.Inst.Input.onMouseClick += OnMouseClick;
        GameManager.Inst.Input.onMouseMove += OnMouseMove;
        GameManager.Inst.Input.onMouseWheel += OnMouseWheel;
    }

    // 상태 관련 함수들 ----------------------------------------------------------------------------

    /// <summary>
    /// 게임 상태가 변경되면 실행될 함수
    /// </summary>
    /// <param name="gameState">현재 게임 상태</param>
    private void OnStateChange(GameState gameState)
    {
        state = gameState;
    }


    // 입력 처리용 함수들 --------------------------------------------------------------------------


    private void OnMouseClick(Vector2 screenPos)
    {
        onMouseClick[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseMove(Vector2 screenPos)
    {
        onMouseMove[(int)state]?.Invoke(screenPos);
    }

    private void OnMouseWheel(float wheelDelta)
    {
        onMouseWheel[(int)state]?.Invoke(wheelDelta);
    }

    // 함선 배치 씬용 입력 함수들 -------------------------------------------------------------------
    private void OnClick_ShipDeployment(Vector2 screen)
    {
        //Debug.Log($"ShipDeployment : Click ({screen.x},{screen.y})");
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (SelectedShip != null && board.ShipDeployment(SelectedShip, world))   // 선택된 배가 있을 때 우선 배치 시도
        {
            Debug.Log("함선배치 성공");
            SelectedShip = null;          // 배 선택 해제
        }
        else
        {
            Debug.Log("배치할 함선이 없거나 실패했습니다.");
        }
    }

    private void OnMouseMove_ShipDeployment(Vector2 screen)
    {
        //Debug.Log($"ShipDeployment : Move ({screen.x},{screen.y})");

        if (SelectedShip != null && !SelectedShip.IsDeployed)               // 선택된 배가 있고 아직 배치가 안된 상황에서만 처리
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(screen);
            world.y = board.transform.position.y;

            if (board.IsInBoard(world))        // 보드 안인지 확인
            {
                Vector2Int grid = board.GetMouseGridPosition();

                // 이동
                SelectedShip.transform.position = board.GridToWorld(grid);            // 보드 안쪽일 때만 위치 이동(칸단위로 이동)

                // 색상 변경
                bool isSuccess = board.IsShipDeplymentAvailable(SelectedShip, grid);  // 배치 가능한지 확인 
                ShipManager.Inst.SetDeloyModeColor(isSuccess);                      // 결과에 따라 색상 변경

            }
            else
            {
                SelectedShip.transform.position = world;      // 자유롭게 움직이기    
                ShipManager.Inst.SetDeloyModeColor(false);  // 밖이면 무조건 빨간 색
            }
        }
    }

    private void OnMouseWheel_ShipDeployment(float wheelDelta)
    {
        //Debug.Log($"ShipDeployment : Wheel ({wheelDelta})");

        bool rotateDir = true;  // 기본값은 시계방향
        if (wheelDelta < 0)         // 입력 방향 확인
        {
            rotateDir = false;  // 입력 방향이 아래쪽이면 반시계방향
        }
        if (SelectedShip != null)
        {
            SelectedShip.Rotate(rotateDir);   // 선택된 배를 회전 시키기

            bool isSuccess = board.IsShipDeplymentAvailable(SelectedShip, SelectedShip.transform.position); // 지금 상태로 배치가 가능한지 확인
            ShipManager.Inst.SetDeloyModeColor(isSuccess);  // 결과에 따라 색상 변경
        }
    }

    // 전투 씬용 입력 함수들 ------------------------------------------------------------------------
    private void OnClick_Battle(Vector2 screen)
    {
        //Debug.Log($"Battle : Click ({screen.x},{screen.y})");
    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 특정 종류의 함선을 선택하는 함수
    /// </summary>
    /// <param name="shipType"></param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = ships[(int)shipType - 1];
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 취소하는 함수
    /// </summary>
    /// <param name="shipType">배치를 취소할 함선</param>
    public void UndoShipDeploy(ShipType shipType)
    {
        Board.UndoShipDeployment(ships[(int)shipType - 1]); // 보드를 이용해서 배치 취소
    }
}
