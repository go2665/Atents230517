using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum PoolObjectType
{
    Shell = 0,
    Explosion
}

public class Factory : Singleton<Factory>
{
    ShellPool shellPool;
    ExplosionPool explosionPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        shellPool = GetComponentInChildren<ShellPool>();
        explosionPool = GetComponentInChildren<ExplosionPool>();

        shellPool?.Initialize();
        explosionPool?.Initialize();
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetObject(PoolObjectType type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Shell:
                result = shellPool?.GetObject(spawn)?.gameObject;                
                break;
            case PoolObjectType.Explosion:
                result = explosionPool?.GetObject(spawn)?.gameObject;
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
    private GameObject GetObject(PoolObjectType type, Vector3 position, float angle = 0.0f)
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

    public TankShellExplosion GetExplosion(Vector3 position, Vector3 normal)
    {
        GameObject obj = GetObject(PoolObjectType.Explosion, position);
        TankShellExplosion explosion = obj.GetComponent<TankShellExplosion>();
        explosion.Initialize(position, normal);
        return explosion;
    }

    public Shell GetShell(Transform parent = null)
    {
        GameObject obj = GetObject(PoolObjectType.Shell, parent);
        Shell shell = obj.GetComponent<Shell>();
        return shell;
    }
}
