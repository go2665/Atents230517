using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    protected int width;
    protected int height;

    protected Cell[] cells;

    public Cell[] MakeMaze(int width, int height, int seed = -1)
    {
        this.width = width;
        this.height = height;
        if(seed != -1)
        {
            Random.InitState(seed);
        }

        cells = new Cell[width * height];

        OnSpecificAlgorithmExcute();

        return cells;
    }

    protected virtual void OnSpecificAlgorithmExcute()
    {
    }

    /// <summary>
    /// 두 셀 사이에 벽을 제거하는 함수
    /// </summary>
    /// <param name="from">시작셀</param>
    /// <param name="to">도착셀</param>
    protected void ConnectPath(Cell from, Cell to)
    {
        Vector2Int diff = new(to.X - from.X, to.Y - from.Y);

        if(diff.x > 0)
        {
            // 동쪽
            from.MakePath(Direction.East);
            to.MakePath(Direction.West);
        }
        else if(diff.x < 0)
        {
            // 서쪽
            from.MakePath(Direction.West);
            to.MakePath(Direction.East);
        }
        else if (diff.y > 0)
        {
            // 남쪽
            from.MakePath(Direction.South);
            to.MakePath(Direction.North);
        }
        else if (diff.y < 0)
        {
            // 북쪽
            from.MakePath(Direction.North);
            to.MakePath(Direction.South);
        }
    }

    protected bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    protected Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }

    protected int GridToIndex(int x, int y)
    {
        return x + y * width;
    }

    protected int GridToIndex(Vector2Int grid)
    {
        return grid.x + grid.y * width;
    }

}
