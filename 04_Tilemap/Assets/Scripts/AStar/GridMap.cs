using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 그리드 1칸 = 1

public class GridMap
{
    /// <summary>
    /// 이 맵에 있는 모든 노드
    /// </summary>
    Node[] nodes;

    /// <summary>
    /// 맵의 가로 길이
    /// </summary>
    int width;

    /// <summary>
    /// 맵의 세로 길이
    /// </summary>
    int height;

    ///// <summary>
    ///// 맵 원점
    ///// </summary>
    //Vector2Int origin;

    /// <summary>
    /// index가 잘못됬다는 것을 표시하는 상수
    /// </summary>
    const int Error_Not_Valid_Position = -1;

    /// <summary>
    /// GridMap 생성자
    /// </summary>
    /// <param name="width">그리드맵의 가로 길이</param>
    /// <param name="height">그리드맵의 세로 길이</param>
    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];   // 가로세로 크기에 맞게 노드 생성

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                //if( GridToIndex(x, y, out int index) )
                //{
                    GridToIndex(x, y, out int index);   // x,y의 범위를 확인할 필요가 없기에 if는 생략
                    nodes[index] = new Node(x, y);      // 인덱스에 맞게 노드 저장
                //}
            }
        }
    }

    /// <summary>
    /// 특정 위치에 있는 노드를 리턴하는 함수
    /// </summary>
    /// <param name="x">맵에서 x위치</param>
    /// <param name="y">맵에서 y위치</param>
    /// <returns>x,y가 정상일 경우 노드의 참조가 리턴, 비정상일 경우 null 리턴</returns>
    public Node GetNode(int x, int y)
    {
        Node node = null;
        if( GridToIndex(x, y, out int index) )
        {
            node = nodes[index];
        }
        return node;
    }

    /// <summary>
    /// 특정 위치에 있는 노드를 리턴하는 함수
    /// </summary>
    /// <param name="gridPos">맵에서 위치</param>
    /// <returns>x,y가 정상일 경우 노드의 참조가 리턴, 비정상일 경우 null 리턴</returns>
    public Node GetNode(Vector2Int gridPos)
    {
        return GetNode(gridPos.x, gridPos.y);
    }

    /// <summary>
    /// 모든 노드의 A* 계산용 데이터 초기화
    /// </summary>
    public void ClearMapData()
    {
        foreach( Node node in nodes )
        {
            node.ClearData();
        }
    }

    /// <summary>
    /// 그리드 위치값을 배열의 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <param name="index">출력용(변환 결과)</param>
    /// <returns>변환 성공여부. x,y가 적절한 값이라 성공이면 true, 아니면 false</returns>
    bool GridToIndex(int x, int y, out int index)
    {
        // 왼쪽 아래가 (0,0)으로 가정하고 작성

        bool result = false;
        index = Error_Not_Valid_Position;
        if( IsValidPosition(x,y) )                  // 적절한 위치인지 판단
        {
            index = x + (height - 1 - y) * width;   // 변환작업
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 그리드 맵 내부인지 아닌지 판단하는 함수
    /// </summary>
    /// <param name="x">그리드맵상의 x위치</param>
    /// <param name="y">그리드맵상의 y위치</param>
    /// <returns>true면 맵 안, false면 맵 바깥</returns>
    public bool IsValidPosition(int x, int y)
    {
        return x < width && y < height && x >= 0 && y >= 0;
    }

    /// <summary>
    /// 그리드 맵 내부인지 아닌지 판단하는 함수
    /// </summary>
    /// <param name="gridPos">그리드맵상의 위치</param>
    /// <returns>true면 맵 안, false면 맵 바깥</returns>
    public bool IsValidPosition(Vector2Int gridPos)
    {
        return IsValidPosition(gridPos.x, gridPos.y);
    }

    // public bool IsWall();
    // public bool IsMonster();

}
