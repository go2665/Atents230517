using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;
    public BackTrackingCell(int x, int y) : base(x, y)
    {
        visited = false;
    }
}

public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);
            }
        }

        // Recurcive Back Tracking(재귀적 백트래킹)
        // 1. 시작지점을 랜덤으로 선택하고 방문했다고 표시한다.
        BackTrackingCell start = (BackTrackingCell)cells[GridToIndex(0, 0)];
        start.visited = true;

        // 재귀문 호출(2, 3 처리)
        MakeRecursive(start.X, start.Y);
        
        // 4. 최종적으로 시작지점까지 돌아가면 알고리즘 종료
    }

    void MakeRecursive(int x, int y)
    {
        BackTrackingCell current = (BackTrackingCell)cells[GridToIndex(x, y)];

        // 2. 그 위치에서 랜덤하게 4방향 중 하나를 선택한다. 단 방문 한적이 없는 셀만 선택할 수 있다.
        Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
        Util.Shuffle(dirs);

        foreach (Vector2Int dir in dirs)
        {
            Vector2Int newPos = new Vector2Int(x + dir.x, y + dir.y);
            if( IsInGrid(newPos) )
            {
                BackTrackingCell neighbor = (BackTrackingCell)cells[GridToIndex(newPos)];
                if( !neighbor.visited )
                {
                    neighbor.visited = true;
                    ConnectPath(current, neighbor);

                    MakeRecursive(neighbor.X, neighbor.Y);
                }    
            }
        }

        // 3. 4방향이 모두 방문되었다면 이전 셀로 돌아간다.
    }
}
