using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    PlayerBullet = 0,       // 플레이어의 총알
    Hit,                    // 플레이어 총알 명중 이팩트
    Explosion,              // 폭팔 이팩트
    Boss,                   // 보스
    BossBullet,             // 보스의 총알
    BossMissle,             // 보스의 유도 미사일
    EnemyAsteroid,          // 운석
    EnemyAsteroidMini,      // 작은 운석
    EnemyCurve,             // 커브로 움직이는 적
    EnemyFighter,           // 물결치듯이 움직이는 적
    EnemyStrike,            // 멈췄다가 돌진하는 적
}

public class Factory : Singleton<Factory>
{
    //public GameObject playerBullet;
    BulletPool bulletPool;
    HitPool hitPool;
    ExplosionPool expsionPool;
    BossPool bossPool;
    BossBulletPool bossBulletPool;
    BossMissiletPool bossMissiletPool;
    EnemyAsteroidPool enemyAsteroidPool;
    EnemyAsteroidMiniPool enemyAsteroidMiniPool;
    EnemyCurvePool enemyCurvePool;
    EnemyFighterPool enemyFighterPool;
    EnemyStrikePool enemyStrikePool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();
        hitPool = GetComponentInChildren<HitPool>();
        expsionPool = GetComponentInChildren<ExplosionPool>();
        bossPool = GetComponentInChildren<BossPool>();
        bossBulletPool = GetComponentInChildren<BossBulletPool>();
        bossMissiletPool = GetComponentInChildren<BossMissiletPool>();
        enemyAsteroidPool = GetComponentInChildren<EnemyAsteroidPool>();
        enemyAsteroidMiniPool = GetComponentInChildren<EnemyAsteroidMiniPool>();
        enemyCurvePool = GetComponentInChildren<EnemyCurvePool>();
        enemyFighterPool = GetComponentInChildren<EnemyFighterPool>();
        enemyStrikePool = GetComponentInChildren<EnemyStrikePool>();

        bulletPool?.Initialize();
        hitPool?.Initialize();
        expsionPool?.Initialize();
        bossPool?.Initialize();
        bossBulletPool?.Initialize();
        bossMissiletPool?.Initialize();
        enemyAsteroidPool?.Initialize();
        enemyAsteroidMiniPool?.Initialize();
        enemyCurvePool?.Initialize();
        enemyFighterPool?.Initialize();
        enemyStrikePool?.Initialize();
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result;
        switch (type)
        {
            case PoolObjectType.PlayerBullet:
                result = bulletPool?.GetObject()?.gameObject;                
                break;
            case PoolObjectType.Hit:
                result = hitPool?.GetObject()?.gameObject; 
                break;
            case PoolObjectType.Explosion:
                    result = expsionPool?.GetObject()?.gameObject;
                break;
            case PoolObjectType.Boss:
                result = bossPool?.GetObject()?.gameObject;
                break;
            case PoolObjectType.BossBullet:
                result = bossBulletPool.GetObject()?.gameObject;
                break;
            case PoolObjectType.BossMissle:
                result = bossMissiletPool.GetObject()?.gameObject;
                break;
            case PoolObjectType.EnemyAsteroid:
                result = enemyAsteroidPool.GetObject()?.gameObject;
                break;
            case PoolObjectType.EnemyAsteroidMini:
                result = enemyAsteroidMiniPool.GetObject()?.gameObject;
                break;
            case PoolObjectType.EnemyCurve:
                result = enemyCurvePool?.GetObject()?.gameObject;
                break;
            case PoolObjectType.EnemyFighter:
                result = enemyFighterPool?.GetObject()?.gameObject;
                break;
            case PoolObjectType.EnemyStrike:
                result = enemyStrikePool?.GetObject()?.gameObject;
                break;
            default:
                result = new GameObject();
                break;
        }

        return result;
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오면서 위치와 각도를 설정하는 함수
    /// </summary>
    /// <param name="type">생성할 오브젝트의 종류</param>
    /// <param name="position">생성할 위치(월드좌표)</param>
    /// <param name="angle">z축 회전 정도</param>
    /// <returns>생성한 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position, float angle = 0.0f)
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.Rotate(angle * Vector3.forward);

        switch (type)
        {            
            case PoolObjectType.EnemyAsteroidMini:
                EnemyAsteroidMini mini = obj.GetComponent<EnemyAsteroidMini>();
                mini.Direction = -mini.transform.right;             // 방향 결정
                break;
            case PoolObjectType.EnemyCurve:
                EnemyCurve curve = obj.GetComponent<EnemyCurve>();
                curve.StartY = position.y;
                break;         
            default:                
                break;
        }
        
        return obj;
    }

    /// <summary>
    /// 작은 운석을 위치와 각도를 설정하면서 받아오는 함수
    /// </summary>
    /// <param name="position">위치 월드 좌표</param>
    /// <param name="angle">z축 회전 정도</param>
    /// <returns>가져온 작은 운석</returns>
    public EnemyAsteroidMini GetAsteroidMini(Vector3 position, float angle = 0.0f)
    {
        EnemyAsteroidMini mini = enemyAsteroidMiniPool.GetObject();
        mini.transform.position = position;                 // 위치 지정
        mini.transform.Rotate(angle * Vector3.forward);     // 회전 시키기
        mini.Direction = -mini.transform.right;             // 방향 결정

        return mini;
    }

    /// <summary>
    /// 커브 도는 적을 받아오는 함수
    /// </summary>
    /// <param name="position">생성되는 위치(월드좌표)</param>
    /// <returns>가져온 커브적</returns>
    public EnemyCurve GetEnemyCurve(Vector3 position)
    {
        EnemyCurve curve = enemyCurvePool.GetObject();
        curve.transform.position = position;
        curve.StartY = position.y;      // 시작 Y 지정
        return curve;
    }
}
