using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_Maze : TestBase
{
    public Direction testDirection = 0;
    public CellDisplayer cellDisplayer;

    public MazeDisplayer mazeDisplayer;
    public int height = 5;
    public int width = 5;
    public int seed = 0;

    protected override void Test1(InputAction.CallbackContext context)
    {
        cellDisplayer.RefreshWall((int)testDirection);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        mazeDisplayer.Clear();

        BackTracking maze = new BackTracking();
        Cell[] cells = maze.MakeMaze(height, width, seed);
        mazeDisplayer.Draw(cells);
    }

}
