using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 축의 기준 : 왼쪽 아래가 원점
// 축의 방향 : 오른쪽 x+, 위쪽 y+

public class Node : IComparable<Node>
{
    /// <summary>
    /// 그리드 맵에서 x좌표
    /// </summary>
    public int x;

    /// <summary>
    /// 그리드 맵에서 y좌표
    /// </summary>
    public int y;

    /// <summary>
    /// A* 알고리즘에서 사용할 G값(출발점에서 이 노드까지 오는데 걸린 실제 거리)
    /// </summary>
    public float G;

    /// <summary>
    /// A* 알고리즘에서 사용할 H값(이 노드에서 목적지까지의 예상 거리)
    /// </summary>
    public float H;

    /// <summary>
    /// G값과 H값의 합(출발점에서 이 노드를 경유해 목적지에 도착할 때 예상되는 거리)
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// A* 알고리즘의 결과 경로에서 앞에 있는 노드
    /// </summary>
    public Node parent;

    public enum NodeType
    {
        Plain,      // 평지(이동 가능)
        Wall,       // 벽(이동 불가능)
        Monster     // 몬스터(이동 불가능)
    }

    public NodeType nodeType = NodeType.Plain;

    /// <summary>
    /// Node의 생성자
    /// </summary>
    /// <param name="x">그리드맵에서 x위치</param>
    /// <param name="y">그리드맵에서 y위치</param>
    /// <param name="nodeType">노드의 종류(기본적으로 평지)</param>
    public Node(int x, int y, NodeType nodeType = NodeType.Plain)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;

        ClearData();
    }

    public void ClearData()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    /// <summary>
    /// 같은 타입 간의 크기 비교를 하는 함수
    /// </summary>
    /// <param name="other">비교 대상</param>
    /// <returns>-1,-0, 1 중 하나</returns>
    public int CompareTo(Node other)
    {
        // 리턴이 0보다 작다(-1)  : 내가 작다(this < other)
        // 리턴이 0이다           : 나와 상대가 같다( this == other )
        // 리턴이 0보다 크다(+1)  : 내가 크다(this > other)

        if (other == null)      // other가 null일 수 있으니 그것에 대한 대비
            return 1;

        return F.CompareTo(other.F);    // F 값을 기준으로 크기를 결정해라.
    }

    /// <summary>
    /// == 명령어 오버로딩. 왼쪽 노드의 위치와 오른쪽 벡터의 (x,y)가 같은지 확인
    /// </summary>
    /// <param name="left">노드</param>
    /// <param name="right">벡터(int)</param>
    /// <returns>true면 같고 false면 다르다.</returns>
    public static bool operator ==(Node left, Vector2Int right)
    {
        return left.x == right.x && left.y == right.y;
    }

    public static bool operator !=(Node left, Vector2Int right)
    {
        return left.x != right.x || left.y != right.y;
    }

    public override bool Equals(object obj)
    {
        return obj is Node other && this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.x, this.y);
    }
}
