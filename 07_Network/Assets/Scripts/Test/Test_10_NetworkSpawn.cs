using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_10_NetworkSpawn : NetTestBase
{
    public GameObject bulletPrefab;
    public GameObject orbPrefab;
    public Transform firePos;

    protected override void Test1(InputAction.CallbackContext context)
    {
        if( IsOwner )
        {
            RequestSpanwBulletServerRpc();
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            RequestEnergyOrbServerRpc();
        }

    }

    [ServerRpc]
    void RequestSpanwBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firePos.position;
        bullet.transform.rotation = firePos.rotation;
        NetworkObject netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn(true);
    }

    [ServerRpc]
    void RequestEnergyOrbServerRpc()
    {
        GameObject energyOrb = Instantiate(orbPrefab);
        energyOrb.transform.position = firePos.position;
        energyOrb.transform.rotation = firePos.rotation;
        NetworkObject netObj = energyOrb.GetComponent<NetworkObject>();
        netObj.Spawn(true);
    }
}


// 실습
// 1. 플레이어가 총알이나 오브와 부딪치면 맵의 랜덤한 지점으로 순간이동 된다.
// 2. 플레이어 공격에 쿨타임 적용하고 UI에도 표시하기