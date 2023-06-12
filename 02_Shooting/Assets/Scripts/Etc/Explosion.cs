using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PooledObject
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StopAllCoroutines();

        // 애니메이션 재생 후에 비활성화 시키기
        StartCoroutine(LifeOver(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }
}
