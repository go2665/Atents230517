using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    PlayerBullet = 0,       // 플레이어의 총알
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

    public GameObject boss;
    public GameObject bossBullet;
    public GameObject bossMissile;
    public GameObject enemyAsteroid;
    public GameObject enemyAsteroidMini;
    public GameObject enemyCurve;
    public GameObject enemyFighter;
    public GameObject enemyStrike;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();

        bulletPool?.Initialize();
    }

    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result;
        switch (type)
        {
            case PoolObjectType.PlayerBullet:
                result = bulletPool?.GetObject()?.gameObject;                
                break;
            case PoolObjectType.Boss:
                result = Instantiate(boss);
                break;
            case PoolObjectType.BossBullet:
                result = Instantiate(bossBullet);
                break;
            case PoolObjectType.BossMissle:
                result = Instantiate(bossMissile);
                break;
            case PoolObjectType.EnemyAsteroid:
                result = Instantiate(enemyAsteroid);
                break;
            case PoolObjectType.EnemyAsteroidMini:
                result = Instantiate(enemyAsteroidMini);
                break;
            case PoolObjectType.EnemyCurve:
                result = Instantiate(enemyCurve);
                break;
            case PoolObjectType.EnemyFighter:
                result = Instantiate(enemyFighter);
                break;
            case PoolObjectType.EnemyStrike:
                result = Instantiate(enemyStrike);
                break;
            default:
                result = new GameObject();
                break;
        }

        return result;
    }
}
