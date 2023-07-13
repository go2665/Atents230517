using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    List<int> loadWork = new List<int>();

    /// <summary>
    /// 로딩 시도가 완료된 목록
    /// </summary>
    [SerializeField]
    List<int> loadWorkComplete = new List<int>();

    /// <summary>
    /// 로딩해제를 시도할 목록
    /// </summary>
    List<int> unloadWork = new List<int>();

    /// <summary>
    /// 로딩해제가 완료된 목록
    /// </summary>
    List<int> unloadWorkComplete = new List<int>();

    /// <summary>
    /// 싱글톤이 처음 만들어졌을 때 단 한번 실행되는 함수
    /// </summary>
    public void PreInitialize()
    {
        sceneNames = new string[HeightCount * WidthCount];              // 배열 크기 확보
        sceneLoadState = new SceneLoadState[HeightCount * WidthCount];

        for(int y = 0; y < HeightCount; y++)
        {
            for(int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}_{x}_{y}"; // 배열 채워넣기
                sceneLoadState[index] = SceneLoadState.Unload;
            }
        }
    }

    /// <summary>
    /// 싱글톤이 만들어지고 씬이 Single로 로드될 때마다 호출될 초기화 함수
    /// </summary>
    public void Initialize()
    {
        for(int i=0;i<sceneLoadState.Length;i++)
        {
            sceneLoadState[i] = SceneLoadState.Unload;  // 씬이 불려졌을 때 서브맵들의 로딩상태 초기화
        }

        Player player = GameManager.Inst.Player;
        if(player != null)
        {
            player.onDie += (_, _) =>   // 플레이어가 죽었을 때 실행될 람다함수(파라메터는 둘 다 무시)
            {
                for(int y =0;y<HeightCount;y++)
                {
                    for(int x =0;x<WidthCount;x++)
                    {
                        RequestAsyncSceneUnload(x, y);  // 모든 씬을 로딩 해제 요청
                    }
                }
            };
            player.onMapMoved += (gridPos) => // 플레이어가 맵을 옮겼을 때 실행될 람다함수
            {
                RefreshScenes(gridPos.x, gridPos.y);    // 플레이어 주변 맵 상태 갱신요청
            };

            Vector2Int grid = WorldToGrid(player.transform.position);   // 플레이어가 있는 서브맵 그리드 가져오기
            RequestAsyncSceneLoad(grid.x, grid.y);  // 플레이어가 있는 맵을 최우선으로 로딩 요청
            RefreshScenes(grid.x, grid.y);          // 플레이어 주변 맵 로딩 요청
        }
    }

    private void Update()
    {
        // 완료된 로딩 작업은 loadWork에서 제거
        foreach(var index in loadWorkComplete)  
        {
            loadWork.RemoveAll( (x) => x == index );// loadWork에 있는 것들 중에서 index와 같은 것을 전부 삭제
        }
        loadWorkComplete.Clear();

        // 로딩 요청 받은 것들을 로딩 시작
        foreach(var index in loadWork)
        {
            AsyncSceneLoad(index);  // loadWork에 있는 것들은 전부 비동기 로딩 시작
        }

        // 완료된 언로드 작업은 unloadWork에서 제거
        foreach (var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index);// unloadWork에 있는 것들 중에서 index와 같은 것을 전부 삭제
        }
        loadWorkComplete.Clear();

        // 로딩 해제 요청 받은 것들을 로딩 시작
        foreach (var index in unloadWork)
        {
            AsyncSceneUnload(index);  // unloadWork에 있는 것들은 전부 비동기 로딩 해제 시작
        }
    }

    /// <summary>
    /// 씬을 비동기로 로딩할 것을 요청하는 함수
    /// </summary>
    /// <param name="x">서브맵의 x위치</param>
    /// <param name="y">서브맵의 y위치</param>
    private void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);
        if(sceneLoadState[index] == SceneLoadState.Unload)
        {
            loadWork.Add(index);
        }
    }

    /// <summary>
    /// 씬을 비동기로 로딩해제 할 것을 요청하는 함수
    /// </summary>
    /// <param name="x">서브맵의 x위치</param>
    /// <param name="y">서브맵의 y위치</param>
    private void RequestAsyncSceneUnload(int x, int y)
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index);  // 작업 리스트에 등록
        }

        // 서브맵에 있는 슬라임을 풀로 되돌리기
        Scene scene = SceneManager.GetSceneByName(sceneNames[index]);   // 씬 찾고
        if(scene.isLoaded)  // 씬이 로딩 상태일 때
        {
            GameObject[] sceneRoots = scene.GetRootGameObjects();   // 부모가 없는 게임오브젝트 전부 찾고
            if( sceneRoots != null && sceneRoots.Length > 0 )
            {
                // 그 중 첫번째(Grid)에서 슬라임 전부 찾기
                Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();    
                foreach(Slime slime in slimes)
                {
                    slime.ReturnToPool();   // 찾은 슬라임을 전부 풀로 되돌리기
                }
            }
        }
    }

    /// <summary>
    /// 비동기 방식으로 씬을 로딩하는 함수
    /// </summary>
    /// <param name="index">로딩할 씬의 인덱스</param>
    private void AsyncSceneLoad(int index)
    {
        if(sceneLoadState[index] == SceneLoadState.Unload)      // Unload 상태일 때만 로딩 시도
        {
            sceneLoadState[index] = SceneLoadState.PendingLoad; // 진행중이라고 표시

            // 비동기 로딩 시작
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);
            async.completed += (_) =>   // 비동기 작업이 끝날 때 실행되는 델리게이트에 람다 함수 추가
            {
                sceneLoadState[index] = SceneLoadState.Loaded;  // 로드 상태로 변경
                loadWorkComplete.Add(index);                    // 로드 완료 목록에 추가
            };
        }
    }

    /// <summary>
    /// 비동기 방식으로 씬을 로딩해제하는 함수
    /// </summary>
    /// <param name="index">로딩해제 할 씬의 인덱스</param>
    private void AsyncSceneUnload(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Loaded)         // 로딩 완료된 상태일 때만 진행
        {
            sceneLoadState[index] = SceneLoadState.PendingUnload;   // 진행중이라고 표시

            // 비동기 언로딩 시작
            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);
            async.completed += (_) =>   // 비동기 작업이 끝날 때 실행되는 델리게이트에 람다 함수 추가
            {
                sceneLoadState[index] = SceneLoadState.Unload;      // 언로드 상태로 변경
                unloadWorkComplete.Add(index);                      // 언로드 완료 목록에 추가
            };
        }
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

    public void TestRefreshScenes(int x, int y)
    {
        RefreshScenes(x, y);
    }
#endif
}
