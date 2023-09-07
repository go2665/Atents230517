using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class NetBullet : NetworkBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 5.0f;
    Rigidbody rigid;

    public int reflectCount = 2;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        rigid.velocity = speed * transform.forward;
        StartCoroutine(SelfDespawn());
    }

    IEnumerator SelfDespawn()
    {
        yield return new WaitForSeconds(lifeTime);

        this.NetworkObject.Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        // 스폰 이후에만 동작
        Debug.Log($"충돌 : {collision.gameObject.name}");
        if(collision.gameObject.CompareTag("Player"))
        {
            this.NetworkObject.Despawn();   // 플레이어라면 즉시 디스폰

            NetPlayer player = collision.gameObject.GetComponent<NetPlayer>();
            player.Die();
        }
        else if( reflectCount > 0 )
        {
            transform.forward = Vector3.Reflect(transform.forward, collision.GetContact(0).normal); // 반사되는 방향으로 회전 수정
            rigid.angularVelocity = Vector3.zero;       // 회전 운동량 제거
            rigid.velocity = speed * transform.forward; // 회전 된 방향으로 운동량도 조절
            reflectCount--;
        }
        else
        {
            this.NetworkObject.Despawn();   // 튕길 수 있는 횟수가 0이하면 즉시 디스폰
        }
    }
}
