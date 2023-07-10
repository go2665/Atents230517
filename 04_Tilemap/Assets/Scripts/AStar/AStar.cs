using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// static은 new가 안됨
public static class AStar
{
    /// <summary>
    /// A* 알고리즘으로 길을 탐색하는 함수
    /// </summary>
    /// <param name="map">길을 찾을 맵</param>
    /// <param name="start">출발지점</param>
    /// <param name="end">도착지점</param>
    /// <returns>출발->도착으로 가는 경로, 길찾기가 실패했을 경우는 null</returns>
    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end)
    {
        const float sideDistance = 1.0f;        // 옆으로 한칸 이동할 때의 거리
        const float diagonalDistance = 1.4f;    // 대각선으로 한칸 이동할 때의 거리

        List<Vector2Int> path = null;

        // 출발지점과 도착지점이 둘다 맵안에 있을때만 길찾기 진행
        if ( map.IsValidPosition(start) && map.IsValidPosition(end) )    
        {
            map.ClearMapData(); // 재사용 때를 대비해서 데이터 초기화

            List<Node> open = new List<Node>();     // open 리스트 만들기(앞으로 탐색할 노드의 리스트)
            List<Node> close = new List<Node>();    // close 리스트 만들기(탐색이 완료된 노드의 리스트)

            // A* 시작하기
            Node current = map.GetNode(start);      // 출발지점 노드를 가져와서 
            current.G = 0;                          // 노드의 G,H 초기 설정
            current.H = GetHeuristic(current, end);
            open.Add(current);                      // 노드를 open리스트에 추가

            // A* 핵심 루틴 시작
            while ( open.Count > 0 )        // open에 노드가 있으면 반복(open리스트가 비면 길찾기 실패)
            {
                open.Sort();                // F기준으로 노드들 정렬
                current = open[0];          // F값이 가장 작은 노드를 current로 설정
                open.RemoveAt(0);           // 꺼낸 노드를 open리스트에서 제거

                if( current != end )        // current가 도착지점인지 확인(도착지점이면 경로 탐색 마무리)
                { 
                    close.Add(current);     // close 리스트에 넣어서 이 노드는 탐색 했음을 표시

                    // current의 주변 8방향 노드를 open리스트에 넣거나 G값 갱신
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(current.x + x, current.y + y);  // 주변 노드 가져오고

                            // 스킵할 노드 체크
                            if (node == null) continue;         // 맵 밖일 때
                            if (node == current) continue;      // 노드가 자기 자신일 때
                            if (node.nodeType == Node.NodeType.Wall) continue;  // 노드가 벽일 때
                            if (close.Exists( (x) => x == node )) continue;     // 노드가 클로즈 리스트에 있을 때
                            bool isDiagonal = (x * y) != 0;     // 대각선인지 확인
                            if (isDiagonal &&   // 대각선이고 대각선 양옆의 노드 중 하나가 벽일 때
                                (map.IsWall(current.x + x, current.y)
                                || map.IsWall(current.x, current.y + y)))
                                continue;

                            // 거리 설정
                            float distance;     
                            if (isDiagonal) 
                                distance = diagonalDistance;    // 대각선이면 거리는 1.4
                            else
                                distance = sideDistance;        // 옆이면 거리는 1

                            // G값을 갱신할지 말지 판단
                            if( node.G > current.G + distance ) // 노드의 G값이 current를 거쳐서 노드로 가는 G값보다 크면 갱신
                            {
                                if( node.parent == null )   // node의 부모가 없으면 아직 open에 안들어간 것으로 판단
                                {
                                    // open리스트에 아직 안들어가 있다.
                                    node.H = GetHeuristic(node, end);   // 휴리스틱 계산하고
                                    open.Add(node);                     // open 리스트에 추가
                                }
                                node.G = current.G + distance;  // G값 갱신
                                node.parent = current;          // current를 통해서 도착했다고 표시
                            }
                        }
                    }
                }
                else
                {
                    break;  // 도착지점에 도착했으니 끝내기
                }
            }

            // 마무리 작업
            if(current == end)  // 목적지에 도착했다면
            {
                path = new List<Vector2Int>();  // 경로 만들기
                Node result = current;
                while(result != null)           // 시작 위치에 도달할때까지 계속 추가
                {
                    path.Add(new Vector2Int(result.x, result.y));
                    result = result.parent;
                }
                path.Reverse(); // 도착->출발로 되어있는 리스트를 뒤집기
            }
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
