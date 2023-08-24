using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if( IsOwner )
        {
            Move();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Vector3 newPos = Random.insideUnitSphere;
            newPos.y = 0;
            position.Value = newPos;
            transform.position = newPos;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Vector3 newPos = Random.insideUnitSphere;
        newPos.y = 0;
        position.Value = newPos;
    }

    private void Update()
    {
        transform.position = position.Value;
    }

    // 네트워크를 통해 플레이어의 이동과 회전을 동기화 시키기

}
