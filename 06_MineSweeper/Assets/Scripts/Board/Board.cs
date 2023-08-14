using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 생성할 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 보드의 가로 길이(셀 개수)
    /// </summary>
    private int width = 16;

    /// <summary>
    /// 보드의 세로 길이(셀 개수)
    /// </summary>
    private int height = 16;

    /// <summary>
    /// 보드에 설치될 지뢰 개수
    /// </summary>
    private int mineCount = 10;

    /// <summary>
    /// 셀 한변의 길이
    /// </summary>
    const float Distance = 1.0f;

    /// <summary>
    /// 이 보드가 가지는 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 열려있는 셀에 표시될 이미지
    /// </summary>
    public Sprite[] openCellImage;
    public Sprite this[OpenCellType type] => openCellImage[(int)type];

    /// <summary>
    /// 닫혀있는 셀에 표시될 이미지
    /// </summary>
    public Sprite[] closeCellImage;
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    /// <summary>
    /// 인풋액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성하고 배치하는 함수.(초기화)
    /// </summary>
    /// <param name="newWidth">보드의 가로 길이</param>
    /// <param name="newHieght">보드의 세로 길이</param>
    /// <param name="newMineCount">보드에 배치될 지뢰수</param>
    public void Initialize(int newWidth, int newHieght, int newMineCount)
    {
        width = newWidth;
        height = newHieght; 
        mineCount = newMineCount;

        if (cells != null)  // 혹시 만들어 진 셀이 있으면
        {
            foreach (var cell in cells)     // 모조리 삭제
            {
                Destroy(cell.gameObject);
            }
            cells = null;
        }

        cells = new Cell[width * height];   // 셀이 들어갈 배열 만들기

        // 셀을 하나씩 생성하기
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);    // 게임 오브젝트 생성
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Board = this;
                cell.ID = x + y * width;                                                    // 셀의 아이디 설정
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);    // 셀의 위치 설정

                cell.onMineSet += MineSet;

                cells[cell.ID] = cell;                          // 배열에 셀 저장
                cellObj.name = $"Cell_{cell.ID}_({x},{y})";     // 셀 게임 오브젝트의 이름 변경
            }
        }

        ResetBoard();
    }

    /// <summary>
    /// 보드에 존재하는 모든 셀의 데이터를 리셋하고 지뢰를 새로 배치하는 함수(게임 재시작용 함수)
    /// </summary>
    private void ResetBoard()
    {
        // 셀의 데이터 초기화
        foreach(var cell in cells)
        {
            cell.ResetData();
        }

        // 보드에 mineCount만큼 지뢰 배치하기
        int[] ids = new int[cells.Length];
        for(int i=0;i < cells.Length;i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for(int i=0;i<mineCount;i++)
        {
            cells[ids[i]].SetMine();
        }
    }

    /// <summary>
    /// 배열을 섞는 함수
    /// </summary>
    /// <param name="source">섞을 배열</param>
    private void Shuffle(int[] source)
    {
        // source의 순서 섞기(피셔-예이츠 알고리즘 사용)
        int loopCount = source.Length - 1;
        for (int i=0;i<loopCount;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, source.Length - i);   // 전체 개수에서 계속 1이 감소하는 범위
            int lastIndex = loopCount - i;  // 마지막에서 계속 1씩 감소하는 숫자

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);    // 스왑하기
        }
    }

    /// <summary>
    /// 특정 셀에 지뢰가 설치되었을 때 처리할 함수
    /// </summary>
    /// <param name="id">셀의 아이디</param>
    private void MineSet(int id)
    {
        Vector2Int grid = IndexToGrid(id);  // 셀의 위치를 찾는다.

        // 위치의 주변 셀을 찾는다.
        for (int y=-1;y<2;y++)
        {
            for(int x = -1;x<2;x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if(index != Cell.ID_NOT_VALID && !(x==0 && y==0))   // 인덱스가 valid하고 (0,0)은 아닌 경우에만 처리
                {
                    Cell cell = cells[index];                       
                    cell.IncreaseAroundMineCount(); // 주변 셀의 aroundMineCount를 1씩 증가 시킨다.
                }
            }
        }
    }

    /// <summary>
    /// 인덱스를 그리즈 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GridToIndex(int x, int y)
    {
        int result = Cell.ID_NOT_VALID;
        if( IsValidGrid(x, y) )
            result = x + y * width;

        return result;
    }

    /// <summary>
    /// 그리드 좌표가 보드 상에 있는 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>true면 적합한 좌표, false면 없는 좌표</returns>
    private bool IsValidGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// 그리드 좌표가 보드 상에 있는 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>true면 적합한 좌표, false면 없는 좌표</returns>
    private bool IsValidGrid(Vector2Int grid)
    {
        return IsValidGrid(grid.x, grid.y);
    }


    // 테스트 함수 ---------------------------------------------------------------------------------
    public void Test_ResetBoard()
    {
        ResetBoard();
    }

    public void Test_Shuffle()
    {
        int[,] result = new int[10, 10];

        for(int i=0;i<1000000;i++)
        {
            int[] source = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Shuffle(source);

            for(int j=0;j<source.Length;j++) 
            {
                result[source[j], j]++;     // (j행, source[j]열)에 1 증가
            }
        }

        string output = "";
        for(int y = 0;y<10;y++)
        {
            output += $"숫자{y} : ";
            for(int x = 0;x<10;x++)
            {
                output += $"{result[y,x]} ";
            }
            output += "\n" ;
        }
        Debug.Log(output);
    }
}

// 2. Cell.SetMine 함수 완성하기