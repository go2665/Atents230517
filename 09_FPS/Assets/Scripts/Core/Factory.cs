using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    BulletHole = 0,       // 총알 구멍
}

public class Factory : Singleton<Factory>
{    
    BulletHolePool bulletHolePool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletHolePool = GetComponentInChildren<BulletHolePool>();

        bulletHolePool?.Initialize();
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObject(PoolObjectType type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.BulletHole:
                result = bulletHolePool?.GetObject(spawn)?.gameObject;                
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
            default:                
                break;
        }
        
        return obj;
    }

    /// <summary>
    /// 총알 구멍을 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public BulletHole GetBulletHole()
    {
        GameObject obj = GetObject(PoolObjectType.BulletHole);
        BulletHole hole = obj.GetComponent<BulletHole>();
        return hole;
    }

    public BulletHole GetBulletHole(Vector3 position, Vector3 normal)
    {
        GameObject obj = GetObject(PoolObjectType.BulletHole);
        BulletHole hole = obj.GetComponent<BulletHole>();
        hole.Initialize(position, normal);
        return hole;
    }
}
