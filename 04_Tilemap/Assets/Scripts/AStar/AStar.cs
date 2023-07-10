using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// static은 new가 안됨
public static class AStar
{
    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end)
    {
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.4f;

        List<Vector2Int> path = null;

        if( map.IsValidPosition(start) && map.IsValidPosition(end) )
        {
            map.ClearMapData();

            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            // A* 시작하기
            Node current = map.GetNode(start);
            current.G = 0;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            // A* 핵심 루틴 시작
            while( open.Count > 0 )
            {
                open.Sort();
                current = open[0];
                open.RemoveAt(0);

                if( current != end ) 
                { 
                    close.Add(current);

                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(current.x + x, current.y + y);
                            if (node == null) continue;
                            if (node == current) continue;
                            if (node.nodeType == Node.NodeType.Wall) continue;
                            if (close.Exists( (x) => x == node )) continue;
                            bool isDiagonal;    
                            // 대각선인지 어떻게 확인할 수 있는가?
                            // 대각선일 때 대각선 양옆의 노드가 벽인지 아닌지는 어떻게 확인 할 수 있는가?
                        }
                    }

                }
                else
                {
                    break;
                }
            }

            // 마무리 작업
        }



        return path;
    }

    /// <summary>
    /// 휴리스틱 값을 계산하는 함수(현재->목적지까지 예상거리 구하기)
    /// </summary>
    /// <param name="current">현재 노드</param>
    /// <param name="end">도착 지점 위치</param>
    /// <returns>current에서 end까지의 예상 거리</returns>
    private static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.x - end.x) + Mathf.Abs(current.y - end.y);
    }
}
