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
}
