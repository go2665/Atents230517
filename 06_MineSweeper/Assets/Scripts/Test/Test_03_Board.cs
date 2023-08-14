using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Board : TestBase
{
    public Board board;

    protected override void Test1(InputAction.CallbackContext context)
    {
        board.Initialize(3, 3, 2);
    }
}
