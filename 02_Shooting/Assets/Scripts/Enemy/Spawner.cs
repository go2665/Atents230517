using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 스폰할 오브젝트의 종류
    /// </summary>
    public PoolObjectType spawnType;

    /// <summary>
    /// 스폰되는 최대 높이
    /// </summary>
    public float maxY = 4;

    /// <summary>
    /// 스폰되는 최소 높이
    /// </summary>
    public float minY = -4;

    /// <summary>
    /// 스폰되는 시간 간격
    /// </summary>
    public float interval = 0.5f;

    /// <summary>
    /// 게임 플레이어
    /// </summary>
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();    // 플레이어 미리 한번만 찾아 놓기
        StartCoroutine(SpawnCoroutine());       // 스폰하는 코루틴 실행
    }

    /// <summary>
    /// 실제로 오브젝트를 스폰하는 함수
    /// </summary>
    /// <returns>스폰한 오브젝트</returns>
    protected virtual EnemyBase Spawn()
    {
        GameObject obj = Factory.Inst.GetObject(spawnType);
        obj.transform.position = new Vector3(       // 위치 변경
            transform.position.x,
            Random.Range(minY, maxY),
            0.0f);

        EnemyBase enemy = obj.GetComponent<EnemyBase>();        
        return enemy;
    }

    /// <summary>
    /// 일정 시간 간격으로 계속 스폰을 하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            Spawn();                                    // 적 스폰하기
            yield return new WaitForSeconds(interval);  // interval에 지정된 시간만큼 기다리기
        }
    }

    /// <summary>
    /// 스폰되는 영역 그리기
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 0.5f, maxY, 0), 
            new Vector3(transform.position.x + 0.5f, maxY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x + 0.5f, maxY, 0), 
            new Vector3(transform.position.x + 0.5f, minY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x + 0.5f, minY, 0), 
            new Vector3(transform.position.x - 0.5f, minY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 0.5f, minY, 0), 
            new Vector3(transform.position.x - 0.5f, maxY, 0));

        //Gizmos.DrawWireCube()
    }

    /// <summary>
    /// 테스트 : 단순 스폰 확인용
    /// </summary>
    public void TestSpawn()
    {
        Spawn();
    }
}
