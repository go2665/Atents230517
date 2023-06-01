using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{
    /// <summary>
    /// 목적지 영역의 기준
    /// </summary>
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);    // 일단 찾기
    }

    /// <summary>
    /// 스폰지역 그리고 도착지점 그리기
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();        // 스폰 지역 그리기

        Gizmos.color = Color.white; // 도착지점 그리기
        if( destination == null )   // play 전에 에러뜨는 것 방지
        {
            destination = transform.GetChild(0);    
        }

        // 도착지점 그리기
        Gizmos.DrawWireCube(destination.position, new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY), 1));
    }

    /// <summary>
    /// 스폰하는 함수(오버라이드)
    /// </summary>
    /// <returns>스폰된 오브젝트</returns>
    protected override EnemyBase Spawn()
    {
        EnemyBase enemy = base.Spawn();     // 일단 스폰 + 초기화

        // is : is 뒤에 있는 타입으로 캐스팅 시도. 실패하면 false, 성공하면 true
        // as : as 뒤에 있는 타입으로 캐스팅 시도. 실패하면 null, 성공하면 참조 리턴
        EnemyAsteroid astroid = enemy as EnemyAsteroid;
        if (astroid != null)
        {
            Vector3 destPos = destination.position;
            destPos.y = Random.Range(minY, maxY);
            astroid.Destination = destPos;  // 도착지점 설정하기
        }
        return astroid;
    }
}
