using System.Collections;
using System.Collections.Generic;
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
        // 1. 시작지검을 랜덤으로 선택하고 방문했다고 표시한다.
        // 2. 그 위치에서 랜덤하게 4방향 중 하나를 선택한다. 단 방문 한적이 없는 셀만 선택할 수 있다.
        // 3. 4방향이 모두 방문되었다면 이전 셀로 돌아간다.
        // 4. 최종적으로 시작지점까지 돌아가면 알고리즘 종료
    }
}
