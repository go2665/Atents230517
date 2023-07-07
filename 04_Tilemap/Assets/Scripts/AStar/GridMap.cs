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

    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                nodes[index] = new Node(x, y);
            }
        }
    }

    int GridToIndex(int x, int y)
    {
        // 구현은 주말과제
        // 왼쪽 아래가 (0,0)으로 가정하고 작성
        return 0;
    }
}
