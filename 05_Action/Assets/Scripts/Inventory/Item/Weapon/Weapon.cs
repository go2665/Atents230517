using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject hitEffect;

    CapsuleCollider bladeCollider;
    ParticleSystem ps;

    Player player;

    private void Awake()
    {
        bladeCollider = GetComponent<CapsuleCollider>();
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    /// <summary>
    /// 칼날 충돌 영역을 켜고 끄는 함수.(적절한 공격 애니메이션 진행 중(임팩트 지점)에 켜지고 꺼짐)
    /// </summary>
    /// <param name="enable">true면 켜고, false면 끈다.</param>
    public void BladeColliderEnable(bool enable)
    {
        bladeCollider.enabled = enable;
    }

    /// <summary>
    /// 칼 잔상 이펙트 켜고 끄는 함수.(공격 애니메이션 중에만 켠다)
    /// </summary>
    /// <param name="enable">true면 재생시작, false면 재생 중지</param>
    public void EffectEnable(bool enable)
    {
        if(enable)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Enemy"))      // 트리거에 들어온 것이 적이고
        {
            IBattle target = other.GetComponent<IBattle>(); // 전투가 가능한 적이라면
            if(target != null )
            {
                player.Attack(target);      // 그 적을 공격

                Vector3 impactPoint = transform.position + transform.up * 0.8f; // 칼날 상단 위치에서
                Vector3 effectPoint = other.ClosestPoint(impactPoint);          // 컬라이더로 가장 가까운 위치를 구해서
                Instantiate(hitEffect, effectPoint, Quaternion.identity);       // 그 위치에 이팩트 생성

                //Time.timeScale = 0.0f;
            }
        }
    }
}
