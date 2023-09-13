using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 한 변의 길이(칸 단위)
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 인덱스가 범위를 벗어났다는 표시
    /// </summary>
    public const int NOT_VALID_INDEX = -1;

    /// <summary>
    /// 보드에 배치되어 있는 배 정보(겹치는 것을 방지하기 위한 정보)
    /// </summary>
    ShipType[] shipInfo;

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];
    }

    /// <summary>
    /// 함선을 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선</param>
    /// <param name="grid">배치될 그리드 좌표(함선 머리 위치)</param>
    /// <returns>배치 성공하면 true, 배치가 불가능하면 false</returns>
    public bool ShipDeployment(Ship ship, Vector2Int grid)
    {
        return false;
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배의 위치(뱃머리 위치)</param>
    /// <param name="resultPos">확실하게 배치할 수 있는 위치(결과가 true일때만 사용)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplyment(Ship ship, Vector2Int grid, out Vector2Int[] resultPos)
    {
        resultPos = null;
        return false;
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배의 위치(뱃머리 위치)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplyment(Ship ship, Vector2Int grid)
    {
        return false;
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="world">확인할 배의 위치(뱃머리 위치, 월드 포지션)</param>
    /// <returns>true면 배치가능한 위치, false면 배치불가능한 위치</returns>
    public bool IsShipDeplyment(Ship ship, Vector3 world)
    {
        return false;
    }

    /// <summary>
    /// 함선 배치 취소 함수
    /// </summary>
    /// <param name="ship">배치를 취소할 배</param>
    public void UndoShipDeployment(Ship ship)
    {

    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="worldPos">월드 좌표</param>
    /// <returns>그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;
        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y+0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>해당 그리드의 가운데 지점 월드좌표</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }    

    /// <summary>
    /// 인덱스를 월드 좌표로 변경해 주는 함수
    /// </summary>
    /// <param name="index">보드 이차원 배열의 인덱스</param>
    /// <returns>인덱스에 해당하는 그리드의 월드 좌표</returns>
    public Vector3 IndexToWorld(int index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 마우스 커서 위치의 그리드좌표를 구해주는 함수
    /// </summary>
    /// <returns>마우스 커서 위치의 그리드좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);

        return WorldToGrid(world);
    }

    /// <summary>
    /// 파라메터로 받은 월드 좌표가 보드 안인지 확인하는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드 좌표</param>
    /// <returns>true면 보드 안쪽, false면 보드 바깥쪽</returns>
    public bool IsInBoard(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;

        return diff.x >= 0.0f && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>결과 그리드 좌표</returns>
    public static Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % BoardSize, index / BoardSize);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드x</param>
    /// <param name="y">그리드y</param>
    /// <returns>해당 위치의 인덱스 값</returns>
    public static int GridToIndex(int x, int y)
    {
        int result = NOT_VALID_INDEX;   // 적절하지 않은 경우 -1
        if (IsVaildGridPosition(x,y))
        {
            result = x + y * BoardSize;
        }

        return result;
    }

    /// <summary>
    /// 그리드 좌표가 적절한지 확인하는 함수
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>true면 적절하고 false면 보드 밖의 좌표</returns>
    public static bool IsVaildGridPosition(int x, int y)
    {
        return x > -1 && x < BoardSize && y > -1 && y <BoardSize;
    }

    /// <summary>
    /// 그리드 좌표가 적절한지 확인하는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>true면 적절하고 false면 보드 밖의 좌표</returns>
    public static bool IsValidGridPosition(Vector2Int grid)
    {
        return IsVaildGridPosition(grid.x, grid.y);
    }
}
