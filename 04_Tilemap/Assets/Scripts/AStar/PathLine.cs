using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //gameObject.SetActive(false);        
    }

    /// <summary>
    /// 경로를 그리는 함수
    /// </summary>
    /// <param name="map">월드 좌표를 구하는데 필요한 맵</param>
    /// <param name="path">맵의 그리드 좌표로 구해진 경로(A* 결과)</param>
    public void DrawPath(GridMap map, List<Vector2Int> path)
    {
        if(map != null && path != null && gameObject.activeSelf)    // 맵과 경로가 있어야 그린다.
        {
            lineRenderer.positionCount = path.Count;    // 경로 갯수 만큼 라인랜더러 위치 갯수 설정

            int index = 0;  // 위치 인덱스
            foreach(Vector2Int node in path)            // 모든 경로를 돌면서 위치 세팅
            { 
                Vector2 worldPos = map.GridToWorld(node);   // 그리드 좌표를 월드 좌표로 변경
                //Vector3 localPos = (Vector3)worldPos - transform.position;    // 라인랜더러에서 월드 좌표사용으로 체크 안되어 있을 경우 계산 필요
                lineRenderer.SetPosition(index, worldPos);  // 라인랜더러의 위치 설정
                index++;    // 인덱스 증가
            }
        }
        else
        {
            lineRenderer.positionCount = 0; // 맵이나 경로가 없거나 비활성화 된 상태면 위치 갯수를 0으로 해서 안보이게 만들기
        }
    }

    /// <summary>
    /// 그려놓은 경로를 초기화 하는 함수
    /// </summary>
    public void ClearPath()
    {
        if(lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 갯수 0으로 해서 안보이게 만들기
        }
        gameObject.SetActive(false);
    }
}
