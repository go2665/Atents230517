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
