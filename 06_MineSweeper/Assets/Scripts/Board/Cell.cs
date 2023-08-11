using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    const int ID_NOT_VALID = -1;
    int id = ID_NOT_VALID;

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

    Board parentBoard;
}
