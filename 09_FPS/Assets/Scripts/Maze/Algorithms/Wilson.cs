using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonCell : Cell
{
    /// <summary>
    /// 경로가 만들어졌을때 다음 셀의 참조.
    /// </summary>
    public WilsonCell next;

    /// <summary>
    /// 이 셀이 미로에 포함되어 있는지 설정하고 확인하는 함수
    /// </summary>
    public bool isMazeMember;

    public WilsonCell(int x, int y) : base(x, y)
    {
        next = null;
        isMazeMember = false;
    }
}

public class Wilson : MazeGenerator
{
    /// <summary>
    /// 4방향 위치값(재활용 용도)
    /// </summary>
    readonly Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };

    protected override void OnSpecificAlgorithmExcute()
    {
        // 모든 셀을 만들기
        for(int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                cells[GridToIndex(x,y)] = new WilsonCell(x,y);
            }
        }

        // 미로에 포함된 셀을 기록하는 리스트 만들기 과정
        int[] notInMazeArray = new int[cells.Length];           // 배열 만들고
        for(int i=0;i<notInMazeArray.Length;i++)
        {
            notInMazeArray[i] = i;                              // 순서대로 번호 넣고
        }
        Util.Shuffle(notInMazeArray);                           // 순서 섞고(랜덤으로 하나씩 나오게 만들기)
        List<int> notInMaze = new List<int>(notInMazeArray);    // 결과를 리스트로 저장

        // 1. 필드의 한곳을 랜덤으로 미로에 추가한다.
        int firstIndex = notInMaze[0];                          // 미로에 포함되지 않은 셀을 하나 꺼내기
        notInMaze.RemoveAt(0);
        WilsonCell first = (WilsonCell)cells[firstIndex];       // 꺼낸 셀을 미로에 포함시키기
        first.isMazeMember = true;

        while(notInMaze.Count > 0)  // 미로에 포함되지 않은 셀이 있으면 계속 반복
        {
            // 2. 미로에 포함되지 않은 셀(A셀)을 랜덤으로 선택한다.
            int index = notInMaze[0];
            notInMaze.RemoveAt(0);
            WilsonCell current = (WilsonCell)cells[index];  // 미로에 포함되지 않은 셀을 하나 골라 current로 지정

            do
            {
                // 3. A셀의 위치에서 랜덤으로 한 칸 움직인다.(B셀, 한칸 움직인 곳)
                //  3.1. 움직인 경로는 기록되어야 한다.
                //  3.2. 움직이다가 이전에 움직였던 경로에 닿을 경우 이전에 진행되었던 경로는 무시한다.
                //  3.3. B셀의 위치가 미로에 포함된 영역에 도착할 때까지 3번으로 돌아가 반복한다.(C셀, 미로에 포함되어 있는 도착한 셀)
                WilsonCell neighbor = GetNeighbor(current);     // current의 이웃 구하기
                current.next = neighbor;                        // current가 어디로 이동하는지 기록
                current = neighbor;                             // 구한 이웃을 새 current로 만들기
            } while (!current.isMazeMember);                    // current가 미로에 포함이 되는 셀이 될 때까지 반복
            
            // 4. B셀 위치에서 C셀까지를 미로에 포함 시킨다.
            //  4.1. 움직인 경로에 따라 길을 만든다.
            WilsonCell path = (WilsonCell)cells[index];         // 처음 찾기 시작했던 셀 가져오기
            while(path != current)                              // 최종 current가 될 때까지 반복
            {
                path.isMazeMember = true;                           // 미로에 포함시키기
                notInMaze.Remove(GridToIndex(path.X, path.Y));      // 미로에 포함되어있지 않은 셀의 목록에서 제거

                ConnectPath(path, path.next);                       // 이번 셀과 다음 셀 사이에 길 쭗기
                path = path.next;                               // 다음 셀로 진행
            }
        } // 5. 모든 셀이 미로에 포함될 때까지 2번으로 돌아가 반복한다.
    }

    /// <summary>
    /// 파라메터로 받은 셀 중 이웃 셀을 하나 선택
    /// </summary>
    /// <param name="cell">이웃을 찾을 셀</param>
    /// <returns>파라메터 셀의 이웃 셀</returns>
    WilsonCell GetNeighbor(WilsonCell cell)
    {
        Vector2Int neighborPos;
        do
        {
            Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
            neighborPos = new Vector2Int(cell.X + dir.x, cell.Y + dir.y);
        } while (!IsInGrid(neighborPos));   // 그리드 영역안에 있는 위치를 고를 때까지 반복

        return (WilsonCell)cells[GridToIndex(neighborPos)];
    }
}
