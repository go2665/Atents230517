using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : PooledObject
{
    // 등장시
    //  - 아웃라인의 두께는 0
    //  - Dissolve의 fade는 1
    //  - Phase의 split은 1
    // 등장 직후에 Phase의 split이 1 -> 0로 변화한다.
    // 플레이어에게 공격 당할 수 있는 상태가 되면 두께는 0.005가 된다.
    // 죽으면 Dislove의 fade가 1 -> 0로 변화한다.

    /// <summary>
    /// 페이즈가 진행되는 시간
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// 페이즈가 끝났음을 알리는 델리게이트
    /// </summary>
    Action onPhaseEnd;

    /// <summary>
    /// 디졸브가 진행되는 시간
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// 디졸브가 끝났음을 알리는 델리게이트
    /// </summary>
    Action onDissolveEnd;

    /// <summary>
    /// 슬라임의 이동 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 이 슬라임이 있는 그리드 맵
    /// </summary>
    GridMap map;

    /// <summary>
    /// 슬라임이 이동할 경로
    /// </summary>
    List<Vector2Int> path = new List<Vector2Int>();

    /// <summary>
    /// 슬라임이 이동할 경로를 그리는 클래스
    /// </summary>
    PathLine pathline;

    /// <summary>
    /// PathLine 읽기 전용 프로퍼티
    /// </summary>
    public PathLine PathLine => pathline;

    /// <summary>
    /// 슬라임의 그리드 좌표 확인용 프로퍼티
    /// </summary>
    Vector2Int GridPosition => map.WorldToGrid(transform.position);

    // 컴포넌트들
    SpriteRenderer spriteRenderer;
    Material mainMaterial;    

    /// <summary>
    /// 아웃라인이 보일때 설정할 두께
    /// </summary>
    const float VisibleOutlineThickness = 0.005f;

    /// <summary>
    /// 페이즈가 진행될 때의 두께
    /// </summary>
    const float VisiblePhaseThickness = 0.1f;

    // 셰이더 프로퍼티용 아이디 구해놓기
    readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplit = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int DissolveFade = Shader.PropertyToID("_DissolveFade");

    private void Awake()
    {
        // 랜더러와 머티리얼 미리 찾아놓기
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;

        onDissolveEnd += ReturnToPool;

        pathline = GetComponentInChildren<PathLine>();
    }

    private void OnEnable()
    {
        ResetShaderProperty();
        StartCoroutine(StartPhase());
    }

    protected override void OnDisable()
    {
        path.Clear();
        PathLine.ClearPath();
     
        base.OnDisable();
    }

    private void Update()
    {
        if( path != null && path.Count > 0 )
        {
            Vector2Int destGrid = path[0];

            Vector3 dest = map.GridToWorld(destGrid);
            Vector3 dir = dest - transform.position;

            if( dir.sqrMagnitude < 0.001f )
            {
                transform.position = dest;
                path.RemoveAt(0);
            }
            else
            {
                transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);
            }
        }
    }

    /// <summary>
    /// 슬라임 초기화용 함수
    /// </summary>
    /// <param name="gridMap">슬라임이 있는 맵</param>
    /// <param name="pos">시작 위치(월드좌표)</param>
    public void Initialize(GridMap gridMap, Vector3 pos)
    {
        map = gridMap;
        transform.position = map.GridToWorld(map.WorldToGrid(pos));
    }

    /// <summary>
    /// 셰이더 프로퍼티 초기화용(재활용을 대비)
    /// </summary>
    void ResetShaderProperty()
    {
        mainMaterial.SetFloat(OutlineThickness, 0.0f);  // 아웃라인 안보이게 두께 조정
        mainMaterial.SetFloat(PhaseSplit, 1.0f);        // 페이즈 초기 상태로(슬라임이 완전히 안보이는 상태)
        mainMaterial.SetFloat(DissolveFade, 1.0f);      // 디졸브 초기 상태로(슬라임이 완전히 보이는 상태)
    }

    /// <summary>
    /// 페이즈를 진행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        float timeElapsed = 0.0f;   // 시간 누적용
        float phaseNormalize = 1.0f / phaseDuration;    // 나누기를 덜하기 위해 미리 계산해 놓은 것
        
        mainMaterial.SetFloat(PhaseThickness, VisiblePhaseThickness);   // 페이즈 선이 보이게 두께 조절

        while (timeElapsed < phaseDuration) // 페이즈가 진행중인 시간이면
        {
            timeElapsed += Time.deltaTime;  // 시간 누적시키고

            // 나누기를 자주하는 것을 피하기 위해 미리 계산해 놓은 phaseNormailze 사용
            mainMaterial.SetFloat(PhaseSplit, 1 - (timeElapsed * phaseNormalize));  // split 조절해서 보이는 영역변경
            yield return null;              // 다음 프레임까지 대기
        }

        // 페이즈가 끝난 상황
        mainMaterial.SetFloat(PhaseThickness, 0.0f);    // 페이즈 선 안보이게 두께 조절
        mainMaterial.SetFloat(PhaseSplit, 0.0f);        // 혹시 -값이 들어갈 수도 있어서 만약을 대비해 0으로 초기화 

        onPhaseEnd?.Invoke();
    }

    /// <summary>
    /// 아웃라인을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 아웃라인을 보여주고 flase면 안보여준다.</param>
    public void ShowOutline(bool isShow = true)
    {
        if(isShow)
        {
            // 보여주는 상황이면 지정된 두께로 지정
            mainMaterial.SetFloat(OutlineThickness, VisibleOutlineThickness);   
        }
        else
        {
            // 안보여주는 상황이면 0으로 지정
            mainMaterial.SetFloat(OutlineThickness, 0.0f);
        }
    }

    /// <summary>
    /// 디졸브 진행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        // 페이즈와 거의 같음
        float timeElapsed = 0.0f;
        float normalize = 1.0f / dissolveDuration;

        while (timeElapsed < dissolveDuration)
        {
            timeElapsed += Time.deltaTime;
            mainMaterial.SetFloat(DissolveFade, 1 - (timeElapsed * normalize));  // 나누기를 자주하는 것을 피하기 위해 미리 계산해 놓은 phaseNormailze 사용
            yield return null;
        }
        mainMaterial.SetFloat(DissolveFade, 0.0f);

        onDissolveEnd?.Invoke();
    }

    /// <summary>
    /// 슬라임이 죽을 때 실행되는 함수
    /// </summary>
    public void Die()
    {        
        StartCoroutine(StartDissolve());    // 디졸브 이팩트 실행
    }

    /// <summary>
    /// 디졸브가 끝났을 때 호출될 함수(디졸브가 끝났을 때 실행되는 델리게이트에 연결되어 있음)
    /// </summary>
    void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 슬라임의 목적지를 설정하는 함수
    /// </summary>
    /// <param name="destination">목적지</param>
    public void SetDestination(Vector2Int destination)
    {
        path = AStar.PathFind(map, GridPosition, destination);
        PathLine.DrawPath(map, path);
    }


    /// <summary>
    /// 테스트용 코드
    /// </summary>
    /// <param name="index"></param>
    public void TestShader(int index)
    {
        switch (index)
        {
            case 1:
                ResetShaderProperty();
                break;
            case 2:
                ShowOutline();
                break;
            case 3:
                ShowOutline(false);
                break;
            case 4:
                StartCoroutine(StartPhase());
                break;
            case 5:
                StartCoroutine(StartDissolve());
                break;
        }
    }
}