using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    CapsuleCollider bladeCollider;
    ParticleSystem ps;

    private void Awake()
    {
        bladeCollider = GetComponent<CapsuleCollider>();
        ps = GetComponent<ParticleSystem>();
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
}
