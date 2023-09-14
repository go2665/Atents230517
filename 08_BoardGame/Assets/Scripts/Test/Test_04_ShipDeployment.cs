using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04_ShipDeployment : TestBase
{
    // 1. 1~5를 눌러서 배를 선택하기(마우스 위치에 배가 붙어서 움직인다.)
    // 2. 보드 안쪽을 좌클릭하면 선택된 배가 배치된다.(겹쳐서 배치되면 안된다. 배치 불가능한 상황이면 머티리얼도 변경되어야 한다.)
    // 3. 우클릭을 하면 그 위치에 있는 배가 배치 해제된다.
    // 4. 휠버튼을 이용해서 배를 배치전에 회전시킬 수 있어야 한다.

    public Board board;
    Ship[] testShips;
    Ship targetShip;
    Ship TargetShip
    {
        get => targetShip; 
        set
        {
            if(targetShip != value)
            {
                if(targetShip != null)
                {
                    targetShip.SetMaterialType();
                    if(!targetShip.IsDeployed)
                    {
                        targetShip.gameObject.SetActive(false);
                    }
                }
            }
            targetShip = value;

            if(targetShip != null)
            {
                targetShip.SetMaterialType(false);

                Vector2 screen = Mouse.current.position.ReadValue();
                Vector3 world = Camera.main.ScreenToWorldPoint(screen);
                world.y = board.transform.position.y;
                targetShip.transform.position = world;
                targetShip.gameObject.SetActive(true);
            }
        }
    }

    private void Start()
    {
        testShips = new Ship[ShipManager.Inst.ShipTypeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Inst.MakeShip(ShipType.Carrier, transform);
        testShips[(int)ShipType.BattleShip - 1] = ShipManager.Inst.MakeShip(ShipType.BattleShip, transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Inst.MakeShip(ShipType.Destroyer, transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Inst.MakeShip(ShipType.Submarine, transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Inst.MakeShip(ShipType.PatrolBoat, transform);        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.MouseMove.performed += OnMouseMove;
        inputActions.Test.R_Click.performed += OnMouseRClick;
        inputActions.Test.MouseWheel.performed += OnMouseWheel;
    }

    protected override void OnDisable()
    {
        inputActions.Test.MouseWheel.performed -= OnMouseWheel;
        inputActions.Test.R_Click.performed -= OnMouseRClick;
        inputActions.Test.MouseMove.performed -= OnMouseMove;
        base.OnDisable();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Ship ship = testShips[(int)ShipType.Carrier - 1];
        if (!ship.IsDeployed)
        {
            Debug.Log("항공모함 선택");
            TargetShip = ship;
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Ship ship = testShips[(int)ShipType.BattleShip - 1];
        if (!ship.IsDeployed)
        {
            Debug.Log("전함 선택");
            TargetShip = ship;
        }
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        Ship ship = testShips[(int)ShipType.Destroyer - 1];
        if (!ship.IsDeployed)
        {
            Debug.Log("구축함 선택");
            TargetShip = ship;
        }
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        Ship ship = testShips[(int)ShipType.Submarine - 1];
        if (!ship.IsDeployed)
        {
            Debug.Log("잠수함 선택");
            TargetShip = ship;
        }
    }

    protected override void Test5(InputAction.CallbackContext context)
    {
        Ship ship = testShips[(int)ShipType.PatrolBoat - 1];
        if (!ship.IsDeployed)
        {
            Debug.Log("경비정 선택");
            TargetShip = ship;
        }
    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if(TargetShip != null && board.ShipDeployment(TargetShip, world))   // 선택된 배가 있을 때 우선 배치 시도
        {
            Debug.Log("함선배치 성공");
            TargetShip = null;          // 배 선택 해제
        }
        else
        {
            Debug.Log("배치할 함선이 없거나 실패했습니다.");
        }
    }

    private void OnMouseWheel(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();

        bool rotateDir = true;  // 기본값은 시계방향
        if( delta < 0 )         // 입력 방향 확인
        {
            rotateDir = false;  // 입력 방향이 아래쪽이면 반시계방향
        }
        if(TargetShip != null)
        {
            TargetShip.Rotate(rotateDir);   // 선택된 배를 회전 시키기

            bool isSuccess = board.IsShipDeplymentAvailable(TargetShip, TargetShip.transform.position); // 지금 상태로 배치가 가능한지 확인
            ShipManager.Inst.SetDeloyModeColor(isSuccess);  // 결과에 따라 색상 변경
        }
    }

    private void OnMouseRClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = board.GetShipType(world);   // 우클릭한 위치의 함선정보 확인
        if(shipType != ShipType.None)                   // 배가 있으면
        {
            Ship ship = testShips[(int)shipType - 1];   // 배 게임오브젝트 가져와서
            board.UndoShipDeployment(ship);             // 보드에서 배치 취소
            ship.gameObject.SetActive(false);           // 배 안보이게 하기
        }
    }

    private void OnMouseMove(InputAction.CallbackContext obj)
    {
        if (TargetShip != null && !TargetShip.IsDeployed)               // 선택된 배가 있고 아직 배치가 안된 상황에서만 처리
        {
            Vector2 screen = Mouse.current.position.ReadValue();
            Vector3 world = Camera.main.ScreenToWorldPoint(screen);
            world.y = board.transform.position.y;

            if( board.IsInBoard(world) )        // 보드 안인지 확인
            {
                Vector2Int grid = board.GetMouseGridPosition();

                // 이동
                TargetShip.transform.position = board.GridToWorld(grid);            // 보드 안쪽일 때만 위치 이동(칸단위로 이동)

                // 색상 변경
                bool isSuccess = board.IsShipDeplymentAvailable(TargetShip, grid);  // 배치 가능한지 확인 
                ShipManager.Inst.SetDeloyModeColor(isSuccess);                      // 결과에 따라 색상 변경

            }
            else
            {
                TargetShip.transform.position = world;      // 자유롭게 움직이기    
                ShipManager.Inst.SetDeloyModeColor(false);  // 밖이면 무조건 빨간 색
            }
        }
    }
}
