using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_PlayerAttack_Func : Test_08_PlayerAttack
{
    public bool isHorizontal = true;

    Vector2Int startGrid;

    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        
        startGrid = enemy.Board.WorldToGrid(world);
        Debug.Log($"Start : {startGrid}");
    }

    protected override void OnR_Click(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        Vector2Int endGrid = enemy.Board.WorldToGrid(world);
        Debug.Log($"End : {endGrid}");

        bool result = user.Test_InSuccessLine(startGrid, endGrid, isHorizontal);
        Debug.Log($"Result : {result}");
    }
}
