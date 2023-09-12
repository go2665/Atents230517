using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Board : TestBase
{
    public Board board;

    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        //Vector2Int grid = board.WorldToGrid(world);
        Vector2Int grid = board.GetMouseGridPosition();
        Debug.Log($"그리드 좌표 : ({grid.x},{grid.y})");

        if( board.IsInBoard(world) )
        {
            Debug.Log("보드의 안쪽입니다.");
        }
        else
        {
            Debug.Log("보드의 바깥쪽입니다.");
        }

        Vector3 center = board.GridToWorld(grid);
        Debug.Log($"이 그리드의 중점 : ({center.x}, {center.y}, {center.z})");
    }
}
