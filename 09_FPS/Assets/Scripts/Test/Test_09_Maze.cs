using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_Maze : TestBase
{
    public Direction testDirection = 0;
    public CellDisplayer cellDisplayer;

    protected override void Test1(InputAction.CallbackContext context)
    {
        cellDisplayer.RefreshWall((int)testDirection);
    }

}
