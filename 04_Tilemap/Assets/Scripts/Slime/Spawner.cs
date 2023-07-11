using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 슬라임 스폰 간격
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// 스포너의 크기(position에서 x(오른쪽),y(위쪽) 크기)
    /// </summary>
    public Vector2Int size;

    /// <summary>
    /// 동시에 최대로 유지 가능한 슬라임 수
    /// </summary>
    public int capacity = 2;

    /// <summary>
    /// 마지막 스폰에서 경과 시간
    /// </summary>
    float elapsed = 0.0f;

    /// <summary>
    /// 현재 스폰된 슬라임 수
    /// </summary>
    int count = 0;

    /// <summary>
    /// 스폰이 가능한 지역
    /// </summary>
    List<Node> spawnAreaList;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        
    }
#endif
}
