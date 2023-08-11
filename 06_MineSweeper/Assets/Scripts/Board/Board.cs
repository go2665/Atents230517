using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// 닫혀있는 셀에 표시될 이미지
    /// </summary>
    public Sprite[] closeCellImage;

    /// <summary>
    /// 인풋액션
    /// </summary>
    PlayerInputActions inputActions;
}
