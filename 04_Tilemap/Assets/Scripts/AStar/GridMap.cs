using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.Image;

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

    /// <summary>
    /// 맵 원점
    /// </summary>
    Vector2Int origin = Vector2Int.zero;

    /// <summary>
    /// 맵을 만드는데 사용한 배경 타일맵(크기 확인 및 계산용)
    /// </summary>
    Tilemap background;

    /// <summary>
    /// index가 잘못됬다는 것을 표시하는 상수
    /// </summary>
    const int Error_Not_Valid_Position = -1;

    /// <summary>
    /// 이동 가능한 지역(평지)의 위치 모음
    /// </summary>
    Vector2Int[] movablePositions;

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
        
        movablePositions = new Vector2Int[width * height];
        
        for (int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                //if( GridToIndex(x, y, out int index) )
                //{
                    GridToIndex(x, y, out int index);   // x,y의 범위를 확인할 필요가 없기에 if는 생략
                    nodes[index] = new Node(x, y);      // 인덱스에 맞게 노드 저장
                    movablePositions[index] = new Vector2Int(x, y);
                //}
            }
        }
    }

    /// <summary>
    /// 타일맵을 이용해 그리드맵을 생성하는 생성자
    /// </summary>
    /// <param name="background">전체 크기를 나타낼 타일맵</param>
    /// <param name="obstacle">못가는 지역을 표시하는 타일맵</param>
    public GridMap(Tilemap background, Tilemap obstacle)
    {
        //backgroud.size.x;     // background의 가로에 셀이 몇개 들어있다.
        //backgroud.size.y;     // background의 세로에 셀이 몇개 들어있다.
        //backgroud.origin;     // background의 왼쪽 아래 셀의 위치가 (,)이다.
        //backgroud.cellBounds.xMin;    // background의 가장 왼쪽 x좌표
        //backgroud.cellBounds.xMax;    // background의 가장 오른쪽 x좌표
        //backgroud.cellBounds.yMin;    // background의 가장 아래쪽 y좌표
        //backgroud.cellBounds.yMax;    // background의 가장 위쪽 y좌표
        //obstacle.GetTile() // 결과가 null이 아니면 벽

        width = background.size.x;  // 가로 크기 저장
        height = background.size.y; // 세로 크기 저장

        origin = (Vector2Int)background.origin; // 원점 위치 기록

        nodes = new Node[width * height];   // 가로세로 크기에 맞게 노드 생성

        // for문이 너무 길어져서 미리 변수로 뽑아놓은 것
        Vector2Int min = new(background.cellBounds.xMin, background.cellBounds.yMin);
        Vector2Int max = new(background.cellBounds.xMax, background.cellBounds.yMax);

        List<Vector2Int> movable = new List<Vector2Int>(width * height);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                GridToIndex(x, y, out int index);           // 인덱스 구하기
                TileBase tile = obstacle.GetTile(new(x, y));
                Node.NodeType tileType = Node.NodeType.Plain;
                if( tile != null)
                {
                    tileType = Node.NodeType.Wall;          // 장애물 타일이 있으면 벽으로 표시
                }
                else
                {
                    movable.Add(new Vector2Int(x, y));
                }
                nodes[index] = new Node(x, y, tileType);    // 노드 생성 후 배열에 저장
            }
        }

        movablePositions = movable.ToArray();
        this.background = background;   // 월드 좌표 계산에 필요해서 저장
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
        // index = x + (height - 1 - y) * width;

        bool result = false;
        index = Error_Not_Valid_Position;
        if( IsValidPosition(x,y) )                  // 적절한 위치인지 판단
        {
            index = (x - origin.x) + (height - 1 - (y - origin.y)) * width;   // 변환작업
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
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
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

    /// <summary>
    /// 입력 받은 위치가 벽인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">x 위치</param>
    /// <param name="y">y 위치</param>
    /// <returns>true면 벽, 아니면 false</returns>
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Wall;
    }

    /// <summary>
    /// 입력 받은 위치가 벽인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="gridPos">위치</param>
    /// <returns>true면 벽, 아니면 false</returns>
    public bool IsWall(Vector2Int gridPos)
    {
        return IsWall(gridPos.x, gridPos.y);
    }

    public bool IsMonster(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Monster;
    }

    public bool IsMonster(Vector2Int gridPos)
    {
        return IsMonster(gridPos.x, gridPos.y);
    }

    /// <summary>
    /// 스폰 가능한 위치인지 확인하는 함수(이동 가능한 지역이 늘어날 것을 대비해서 작성)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsSpawnable(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Plain;
    }

    public bool IsSpawnable(Vector2Int gridPos)
    {
        return IsSpawnable(gridPos.x, gridPos.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="worldPos">월드 좌표</param>
    /// <returns>월드 좌표가 변경된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        if( background != null )
        {
            return (Vector2Int)background.WorldToCell(worldPos);    // 타일맵이 있으면 타일맵이 제공하는 함수 사용
        }

        return new Vector2Int((int)worldPos.x, (int)worldPos.y);    // (0,0)을 기준으로 계산
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="gridPos">그리드 좌표</param>
    /// <returns>그리드 좌표가 변경된 월드 좌표</returns>
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        if( background != null )
        {
            return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f,0.5f);
        }

        return new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);
    }

    /// <summary>
    /// 랜덤으로 이동 가능한 지역을 하나 리턴하는 함수
    /// </summary>
    /// <returns>이동 가능한 위치(그리드 좌표)</returns>
    public Vector2Int GetRandomMovablePosition()
    {
        int index = UnityEngine.Random.Range(0, movablePositions.Length);
        return movablePositions[index];
    }

}
