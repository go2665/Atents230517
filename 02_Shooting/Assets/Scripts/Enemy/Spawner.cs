using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 스폰 정보를 저장하는 구조체
    [Serializable]
    public struct SpawnData
    {
        public SpawnData(PoolObjectType type = PoolObjectType.EnemyFighter, float interval = 0.5f)
        {
            this.spawnType = type;
            this.interval = interval;
        }

        /// <summary>
        /// 스폰할 오브젝트의 종류
        /// </summary>
        public PoolObjectType spawnType;

        /// <summary>
        /// 스폰되는 시간 간격
        /// </summary>
        public float interval;

        // 시작 딜레이 추가
    }

    /// <summary>
    /// 스폰 데이터들을 가지는 배열
    /// </summary>
    //public List<SpawnData> spawnDatas; 
    public SpawnData[] spawnDatas;

    /// <summary>
    /// 스폰되는 최대 높이
    /// </summary>
    public float maxY = 4;

    /// <summary>
    /// 스폰되는 최소 높이
    /// </summary>
    public float minY = -4;    

    /// <summary>
    /// 게임 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 목적지 영역의 기준
    /// </summary>
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);    // 일단 찾기
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();    // 플레이어 미리 한번만 찾아 놓기

        foreach (var spawnData in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(spawnData));  // 스폰하는 코루틴 실행
        }
    }

    /// <summary>
    /// 실제로 오브젝트를 스폰하는 함수
    /// </summary>
    /// <returns>스폰한 오브젝트</returns>
    protected virtual EnemyBase Spawn(PoolObjectType spawnType)
    {
        GameObject obj = Factory.Inst.GetObject(spawnType,
            new Vector3(transform.position.x, UnityEngine.Random.Range(minY, maxY), 0.0f));
        EnemyBase enemy = obj.GetComponent<EnemyBase>();
        
        switch(spawnType)
        {
            case PoolObjectType.EnemyAsteroid:
                Vector3 destPos = destination.position; // 목적지 x는 destination의 x 사용
                destPos.y = UnityEngine.Random.Range(minY, maxY);   // 목적지 y 랜덤으로 정하기
                EnemyAsteroid astroid = enemy as EnemyAsteroid;
                astroid.Destination = destPos;          // 도착지점 설정하기
                break;
        }

        return enemy;
    }

    /// <summary>
    /// 일정 시간 간격으로 계속 스폰을 하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCoroutine(SpawnData data)
    {
        while(true)
        {
            Spawn(data.spawnType);                          // 적 스폰하기
            yield return new WaitForSeconds(data.interval); // interval에 지정된 시간만큼 기다리기
        }
    }

    /// <summary>
    /// 스폰되는 영역 그리기
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        // 출발지점 그리기
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

        // 도착지점 그리기
        Gizmos.color = Color.white; 
        if (destination == null)   // play 전에 에러뜨는 것 방지
        {
            destination = transform.GetChild(0);
        }
        Gizmos.DrawWireCube(destination.position, new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY), 1));
    }

    ///// <summary>
    ///// 테스트 : 단순 스폰 확인용
    ///// </summary>
    //public void TestSpawn()
    //{
    //    Spawn();
    //}
}
