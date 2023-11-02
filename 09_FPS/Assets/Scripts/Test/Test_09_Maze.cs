using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_Maze : TestBase
{
    public Direction testDirection = 0;
    public CellVisualizer cellVisualizer;

    public MazeVisualizer mazeVisualizer_Backtracking;
    public MazeVisualizer mazeVisualizer_Eller;
    public MazeVisualizer mazeVisualizer_Wilson;
    public int height = 5;
    public int width = 5;
    public int seed = 0;

    protected override void Test1(InputAction.CallbackContext context)
    {
        cellVisualizer.RefreshWall((int)testDirection);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        mazeVisualizer_Backtracking.Clear();

        BackTracking maze = new BackTracking();
        Cell[] cells = maze.MakeMaze(height, width, seed);
        mazeVisualizer_Backtracking.Draw(cells, maze.Goal);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        mazeVisualizer_Eller.Clear();

        Eller maze = new Eller();
        Cell[] cells = maze.MakeMaze(height, width, seed);
        mazeVisualizer_Eller.Draw(cells, maze.Goal);
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        mazeVisualizer_Wilson.Clear();

        Wilson maze = new Wilson();
        Cell[] cells = maze.MakeMaze(height, width, seed);
        mazeVisualizer_Wilson.Draw(cells, maze.Goal);
    }

}
