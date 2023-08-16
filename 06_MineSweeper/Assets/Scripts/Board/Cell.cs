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
    CellMarkState MarkState
    {
        get => markState;
        set
        {
            markState = value;
            switch (markState)
            {
                case CellMarkState.None:
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                case CellMarkState.Flag:
                    cover.sprite = Board[CloseCellType.Flag];
                    break;
                case CellMarkState.Question:
                    cover.sprite = Board[CloseCellType.Question];
                    break;
                default:
                    break;
            }

        }
    }

    /// <summary>
    /// 깃발 설치 여부를 확인하는 프로퍼티
    /// </summary>
    public bool IsFlaged => markState == CellMarkState.Flag;

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

    /// <summary>
    /// 셀에 깃발이 설치되었다고 알리는 델리게이트
    /// </summary>
    public Action onFlagUse;

    /// <summary>
    /// 셀에 설치된 깃발을 제거했다고 알리는 델리게이트
    /// </summary>
    public Action onFlagReturn;

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

    /// <summary>
    /// 이 셀을 여는 함수
    /// </summary>
    private void Open()
    {
        if( !isOpen && !IsFlaged )
        {
            isOpen = true;
            cover.gameObject.SetActive(false);
            if( hasMine )
            {
                inside.sprite = Board[OpenCellType.Mine_Explotion];
                // 게임 오버 처리
            }
            else if(aroundMineCount == 0)
            {
                // 주변 셀을 모두 열기
            }
        }
    }

    /// <summary>
    /// 셀을 마우스 왼쪽 버튼으로 눌렀을 때 실행되는 함수
    /// </summary>
    public void CellLeftPress()
    {
        // 눌린 표시를 한다.
        if(isOpen)
        {
            // 주변 8개 셀 중에 닫혀있는 셀만 누르는 표시를 한다.
        }
        else
        {
            // 이 셀만 누르고 있는 표시를 한다.
            switch (MarkState)
            {
                case CellMarkState.None:
                    cover.sprite = Board[CloseCellType.Close_Press];
                    break;
                case CellMarkState.Question:
                    cover.sprite = Board[CloseCellType.Question_Press];
                    break;
                case CellMarkState.Flag:
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 셀을 마우스 왼쪽 버튼으로 눌렀다 땠을 때 실행되는 함수
    /// </summary>
    public void CellLeftRelease()
    {
        if(isOpen)
        {
            // 셀에 기록된 깃발 개수와 주변에 설치된 깃발의 개수가 같으면 
            //      주변 셀 중에서 닫혀있는 셀은 모두 연다.
            //      아니면 다시 모두 원상복구

        }
        else
        {
            // 이 셀을 연다.
            Open();
        }
    }
    //1. 셀을 열때 주변 지뢰 개수가 0이면 주변셀을 모두 열기
    //2. 열려있는 셀을 눌렀을 경우 주변 8개 셀 중 닫혀있는 셀은 모두 눌린 표시를 한다.
    //3. 2번 상태일 때 마우스를 때면 주변 깃발 개수로 주변 지뢰 개수가 같으면 깃발 표시된 셀을 제외하고 모두 연다


    /// <summary>
    /// 셀을 마우스 오른쪽 버튼으로 눌렀을 때 실행되는 함수
    /// </summary>
    public void CellRightPress()
    {
        // markState에 따라 우클릭 되었을 때 cover의 이미지 변경하기(프로퍼티 설정하면서 이미지 자동 변경)
        switch (MarkState)
        {
            case CellMarkState.None:
                MarkState = CellMarkState.Flag;     
                onFlagUse?.Invoke();
                break;
            case CellMarkState.Flag:
                MarkState = CellMarkState.Question;
                onFlagReturn?.Invoke();
                break;
            case CellMarkState.Question:
                MarkState = CellMarkState.None;
                break;
            default:
                break;
        }
    }

    public void RestoreCover()
    {
        switch (MarkState)
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close];
                break;
            case CellMarkState.Question:
                cover.sprite = Board[CloseCellType.Question];
                break;
            case CellMarkState.Flag:
            default:
                break;
        }
    }
}


