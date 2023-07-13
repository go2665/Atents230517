using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    /// <summary>
    /// 서브맵의 세로 개수
    /// </summary>
    const int HeightCount = 3;

    /// <summary>
    /// 서브맵의 가로 개수
    /// </summary>
    const int WidthCount = 3;

    /// <summary>
    /// 서브맵 하나의 세로 길이
    /// </summary>
    const float mapHeightLength = 20.0f;

    /// <summary>
    /// 서브맵 하나의 가로 길이
    /// </summary>
    const float mapWidthLength = 20.0f;

    /// <summary>
    /// 월드의 원점(왼쪽 아래가 새 기준)
    /// </summary>
    readonly Vector2 worldOrigin = new Vector2(
        -mapWidthLength * WidthCount * 0.5f, -mapHeightLength * HeightCount * 0.5f);

    /// <summary>
    /// 씬 이름 조합용 기본 이름
    /// </summary>
    const string SceneNameBase = "Seemless";

    /// <summary>
    /// 조합이 완료된 모든 씬의 이름 배열
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// 씬의 로딩 상태를 나타낼 enum 타입
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload = 0,     // 로딩이 안되어 있음
        PendingUnload,  // 로딩 해제 진행 중
        PendingLoad,    // 로딩 진행 중
        Loaded          // 로딩 완료됨
    }

    /// <summary>
    /// 모든 씬의 로딩 상태
    /// </summary>
    SceneLoadState[] sceneLoadState;

    /// <summary>
    /// 모든 씬이 언로드 되었음을 확인하기 위한 프로퍼티. 모든 씬이 Unload일때만 true, 아니면 false
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach (SceneLoadState state in sceneLoadState)
            {
                if(state != SceneLoadState.Unload)
                {
                    result = false;     // 하나라도 Unload가 아니면 false
                    break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 로딩을 시도할 목록
    /// </summary>
    List<int> loadWork = new List<int>();

    /// <summary>
    /// 로딩 시도가 완료된 목록
    /// </summary>
    List<int> loadWorkComplete = new List<int>();

    /// <summary>
    /// 로딩해제를 시도할 목록
    /// </summary>
    List<int> unloadWork = new List<int>();

    /// <summary>
    /// 로딩해제가 완료된 목록
    /// </summary>
    List<int> unloadWorkComplete = new List<int>();


    public void PreInitialize()
    {

    }

    public void Initialize()
    {

    }

    /// <summary>
    /// 씬을 비동기로 로딩할 것을 요청하는 함수
    /// </summary>
    /// <param name="x">서브맵의 x위치</param>
    /// <param name="y">서브맵의 y위치</param>
    private void RequestAsyncSceneLoad(int x, int y)
    {

    }

    /// <summary>
    /// 씬을 비동기로 로딩해제 할 것을 요청하는 함수
    /// </summary>
    /// <param name="x">서브맵의 x위치</param>
    /// <param name="y">서브맵의 y위치</param>
    private void RequestAsyncSceneUnload(int x, int y)
    {

    }

    /// <summary>
    /// 비동기 방식으로 씬을 로딩하는 함수
    /// </summary>
    /// <param name="index">로딩할 씬의 인덱스</param>
    private void AsyncSceneLoad(int index)
    {

    }

    /// <summary>
    /// 비동기 방식으로 씬을 로딩해제하는 함수
    /// </summary>
    /// <param name="index">로딩해제 할 씬의 인덱스</param>
    private void AsyncSceneUnload(int index)
    {

    }

    /// <summary>
    /// 지정된 위치 주변 맵은 로딩 요청하고 그 외는 로딩해제를 요청하는 함수
    /// </summary>
    /// <param name="gridX">지정된 x위치</param>
    /// <param name="gridY">지정된 y위치</param>
    private void RefreshScenes(int gridX, int gridY)
    {

    }

    /// <summary>
    /// 서브맵의 그리드 위치를 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x">서브맵의 x위치</param>
    /// <param name="y">서브맵의 y위치</param>
    /// <returns>위치를 하나의 숫자로 변경한 인덱스(배열에서 사용)</returns>
    private int GetIndex(int x, int y)
    {
        return x + WidthCount * y;
    }

    /// <summary>
    /// 월드 좌표가 어떤 서브맵이 있는 그리드인지 계산하는 함수(플레이어가 어느 서버맵에 있는지 확인용)
    /// </summary>
    /// <param name="worldPos">월드 좌표</param>
    /// <returns>서브맵의 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector2 offset = (Vector2)worldPos - worldOrigin;
        return new Vector2Int((int)(offset.x / mapWidthLength), (int)(offset.y / mapHeightLength));
    }

    // 테스트용 함수 -------------------------------------------------------------------------------
#if UNITY_EDITOR
    public void TestLoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void TestUnloadScene(int x, int y)
    {
        RequestAsyncSceneUnload(x, y);
    }
#endif
}
