using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_07_PlayerBase : TestBase
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;
    public PlayerBase player;

    Board board;

    private void Start()
    {
        reset.onClick.AddListener(player.UndoAllShipDeployment);
        random.onClick.AddListener(() => player.AutoShipDeployment(true));
        resetAndRandom.onClick.AddListener(() =>
        {
            player.UndoAllShipDeployment();
            player.AutoShipDeployment(true);
        });

        board = player.Board;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.R_Click.performed += OnR_Click;
    }

    protected override void OnDisable()
    {
        inputActions.Test.R_Click.performed -= OnR_Click;
        base.OnDisable();
    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        // 클릭한 지점 공격
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        board.OnAttacked(board.WorldToGrid(world));
    }

    private void OnR_Click(InputAction.CallbackContext context)
    {
        // 우클릭한 지점에 있는 함선 배치 취소
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        ShipType shipType = board.GetShipType(world);   // 우클릭한 위치의 함선정보 확인
        if (shipType != ShipType.None)                  // 배가 있으면
        {
            Ship ship = player.GetShip(shipType);       // 배 게임오브젝트 가져와서
            board.UndoShipDeployment(ship);             // 보드에서 배치 취소
            ship.gameObject.SetActive(false);           // 배 안보이게 하기
        }
    }


}
