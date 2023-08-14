using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    /// <summary>
    /// ID가 정상이 아니라는 것을 알리기 위한 상수
    /// </summary>
    public const int ID_NOT_VALID = -1;

    /// <summary>
    /// 이 셀의 ID(위치 계산에도 사용 가능)
    /// </summary>
    int id = ID_NOT_VALID;

    /// <summary>
    /// ID 확인 및 설정용 프로퍼티
    /// </summary>
    public int ID
    {
        get => id;
        set
        {
            if( id == ID_NOT_VALID )    // ID가 아직 설정되지 않았을 때만 가능
            {
                id = value;
            }
        }
    }

    /// <summary>
    /// 겉면 표시용 스프라이트
    /// </summary>
    SpriteRenderer cover;

    /// <summary>
    /// 안쪽면 표시용 스프라이트
    /// </summary>
    SpriteRenderer inside;

    /// <summary>
    /// 셀의 표시 상태용 enum(닫혔을 때 한정)
    /// </summary>
    enum CellMarkState
    {
        None = 0,   // 아무것도 표시 안되어있음
        Flag,       // 깃발 표시됨
        Question    // 물음표 표시됨
    }

    /// <summary>
    /// 지금 현재 표시된 마크
    /// </summary>
    CellMarkState markState = CellMarkState.None;

    /// <summary>
    /// 셀이 열려있는지 여부(true면 열려있고, false면 닫혀있다.)
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// 셀에 지뢰가 있는지 여부(true면 지뢰가 있고, false면 없다)
    /// </summary>
    bool hasMine = false;

    /// <summary>
    /// 이 셀 주변에 있는 지뢰의 개수
    /// </summary>
    int aroundMineCount = 0;

    /// <summary>
    /// 이 셀이 존재하는 보드
    /// </summary>
    Board parentBoard = null;

    /// <summary>
    /// 보드에 접근하거나 설정하기 위한 프로퍼티
    /// </summary>
    public Board Board
    {
        get => parentBoard;
        set
        {
            if (parentBoard == null)    // 설정은 한번만 가능
            {
                parentBoard = value;
            }
        }
    }

    /// <summary>
    /// 셀에 지뢰가 설치되었다고 알리는 델리게이트
    /// </summary>
    public Action<int> onMineSet;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        cover = child.GetComponent<SpriteRenderer>();
        child = transform.GetChild(1);
        inside = child.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 이 셀의 데이터를 초기화하는 함수
    /// </summary>
    public void ResetData()
    {
        markState = CellMarkState.None;
        isOpen = false;
        hasMine = false;
        aroundMineCount = 0;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);   // 다시 닫는 함수는 없음
    }

    /// <summary>
    /// 이 셀에 지뢰를 설치하는 함수
    /// </summary>
    public void SetMine()
    {
        hasMine = true;         // 지뢰 설치되었다고 표시
        inside.sprite = Board[OpenCellType.Mine_NotFound];  //기본 지뢰 스프라이트 설정
        
        onMineSet?.Invoke(ID);  // 델리게이트로 알림
    }

    /// <summary>
    /// 이 셀이 지뢰가 배치되어 있지 않을 때 AroundMineCount를 1 증가 시키는 함수
    /// </summary>
    public void IncreaseAroundMineCount()
    {
        if(!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];
        }
    }
}
