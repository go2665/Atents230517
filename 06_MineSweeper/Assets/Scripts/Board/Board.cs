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

    private void Shuffle(int[] source)
    {
        // source의 순서 섞기
    }
}

// 1. Shuffle 함수 완성하기
// 2. Cell.SetMine 함수 완성하기