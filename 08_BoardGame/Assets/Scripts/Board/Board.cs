using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 한 변의 길이(칸 단위)
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="worldPos">월드 좌표</param>
    /// <returns>그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return Vector2Int.zero;
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return Vector3.zero;
    }    

    /// <summary>
    /// 인덱스를 월드 좌표로 변경해 주는 함수
    /// </summary>
    /// <param name="index">보드 이차원 배열의 인덱스</param>
    /// <returns>인덱스에 해당하는 그리드의 월드 좌표</returns>
    public Vector3 IndexToWorld(int index)
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 마우스 커서 위치의 그리드좌표를 구해주는 함수
    /// </summary>
    /// <returns>마우스 커서 위치의 그리드좌표</returns>
    public Vector2Int GetMouseGridPosision()
    { 
        return Vector2Int.zero; 
    }

    /// <summary>
    /// 파라메터로 받은 월드 좌표가 보드 안인지 확인하는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드 좌표</param>
    /// <returns>true면 보드 안쪽, false면 보드 바깥쪽</returns>
    public bool IsValidPosition(Vector3 worldPos)
    {
        return false;
    }

}
