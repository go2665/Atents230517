using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    const int ID_NOT_VALID = -1;
    int id = ID_NOT_VALID;
    public int ID
    {
        get => id;
        set
        {
            if( id == ID_NOT_VALID )
            {
                id = value;
            }
        }
    }

    SpriteRenderer cover;
    SpriteRenderer inside;

    enum CellMarkState
    {
        None = 0,   // 아무것도 표시 안되어있음
        Flag,       // 깃발 표시됨
        Question    // 물음표 표시됨
    }

    CellMarkState markState = CellMarkState.None;

    bool isOpen = false;
    bool hasMine = false;

    int aroundMineCount = 0;

    Board parentBoard = null;
    public Board Board
    {
        get => parentBoard;
        set
        {
            if (parentBoard == null)
            {
                parentBoard = value;
            }
        }
    }

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

    public void SetMine()
    {
        hasMine = true;
        // 스프라이트 지뢰모양으로 변경
        // 주변 셀의 aroundMineCount 증가
    }
}
