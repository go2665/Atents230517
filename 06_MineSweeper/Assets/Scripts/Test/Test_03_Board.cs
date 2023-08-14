using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Board : TestBase
{
    public Board board;
    public int width = 16;
    public int height = 16;
    public int mineCount = 10;

    protected override void Test1(InputAction.CallbackContext context)
    {
        board.Initialize(width, height, mineCount);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        board.Test_ResetBoard();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        board.Test_Shuffle();
    }
}
