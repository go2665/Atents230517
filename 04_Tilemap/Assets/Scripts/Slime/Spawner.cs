using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public Vector2 size;

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

    /// <summary>
    /// 맵 매니저(스포너도 관리)
    /// </summary>
    MapManager mapManager;

    private void Start()
    {
        mapManager = GetComponentInParent<MapManager>();
        spawnAreaList = mapManager.CalcSpawnArea(this);
    }

    private void Update()
    {
        if(count < capacity)
        {
            elapsed += Time.deltaTime;
            if(elapsed > interval)
            {
                Spawn();
                elapsed = 0.0f;
            }
        }
    }

    private void Spawn()
    {
        if(GetSpawnPosition(out Vector3 spawnPos))
        {
            //Factory.Inst.
        }
    }

    private bool GetSpawnPosition(out Vector3 spawnPos)
    {
        bool result = false;
        List<Node> spawns = new List<Node>();

        foreach(Node node in spawnAreaList)
        {
            if( node.nodeType == Node.NodeType.Plain )
            {
                spawns.Add(node);
            }
        }

        if(spawns.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, spawns.Count);
            Node target = spawns[index];
            spawnPos = mapManager.GridToWorld(target.x, target.y);
            result = true;
        }
        else
        {
            spawnPos = Vector3.zero;
        }

        return result;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 basePos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector3 p0 = basePos;
        Vector3 p1 = basePos + Vector3.right * size.x;
        Vector3 p2 = basePos + new Vector3(size.x, size.y);
        Vector3 p3 = basePos + Vector3.up * size.y;

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }
#endif
}
