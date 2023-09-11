using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class NetEnergyOrb : NetworkBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 20.0f;
    Rigidbody rigid;

    VisualEffect vfx;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        vfx = GetComponentInChildren<VisualEffect>();
    }

    public override void OnNetworkSpawn()
    {
        transform.Rotate(-30.0f, 0.0f, 0.0f);
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
        //vfx.SetFloat("Size", 5);
        
        if(collision.gameObject.CompareTag("Player"))
        {
            NetPlayer player = collision.gameObject.GetComponent<NetPlayer>();
            player.Die();
        }

        // 실습
        // 오브가 터진 위치에서 일정 반경 안에 있는 플레이어는 리스폰된다.
        
        EffectProcessClientRpc();
    }

    /// <summary>
    /// ClientRpc : 서버가 모든 클라이언트에게 로컬에서 실행하라고 하는 함수
    /// </summary>
    [ClientRpc]
    void EffectProcessClientRpc()
    {
        rigid.drag = float.MaxValue;
        rigid.angularDrag = float.MaxValue;
        StartCoroutine(EffectProcess());
    }

    IEnumerator EffectProcess()
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < 0.5f)           // 0.5초 동안 최대치까지 확대
        {
            elapsedTime += Time.deltaTime;
            vfx.SetFloat("Size", elapsedTime * 10.0f);  // 최대 5까지 증가
            yield return null;
        }
        elapsedTime = 1.0f;
        while(elapsedTime > 0.0f)           // 1초 동안 크기 감소
        {
            elapsedTime -= Time.deltaTime;
            vfx.SetFloat("Size", elapsedTime * 5.0f);   // 5 -> 0이 될때까지
            yield return null;
        }

        vfx.SendEvent("OnEffectFinish");    // 크기 감소가 끝나면 파티클 생성 중지

        while(vfx.aliveParticleCount > 0)   // 살아있는 파티클이 없을 때까지 대기
        {            
            yield return null;
        }
        this.NetworkObject.Despawn();       // 살아있는 파티클이 없으면 디스폰
    }
}