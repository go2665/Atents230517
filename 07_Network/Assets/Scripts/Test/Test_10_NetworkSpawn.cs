using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_10_NetworkSpawn : NetTestBase
{
    public GameObject bulletPrefab;
    public Transform firePos;

    protected override void Test1(InputAction.CallbackContext context)
    {
        if( IsOwner )
        {
            RequestSpanwBulletServerRpc();
        }
    }

    [ServerRpc]
    void RequestSpanwBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firePos.position;
        NetworkObject netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn(true);
    }
}
