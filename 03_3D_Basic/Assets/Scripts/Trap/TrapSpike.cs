using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : TrapBase
{
    // 밟으면 바닥에서 가시가 올라오며 플레이어가 죽는 함정

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);
        animator.SetTrigger("Activate");    // 함정이 발동될 때 애니메이션 재생
    }
}
