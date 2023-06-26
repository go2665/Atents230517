using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPush : TrapBase
{
    public float pushPower = 5.0f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);
        animator.SetTrigger("Activate");    // 함정이 발동될 때 애니메이션 재생

        Rigidbody targetRigid = target.GetComponent<Rigidbody>();
        Player player = target.GetComponent<Player>();
        if (player != null && targetRigid != null)
        {
            Vector3 dir = (transform.up - transform.forward).normalized;
            targetRigid.AddForce(pushPower * dir, ForceMode.Impulse);
            player.SetForceJumpMode();
        }
    }
}
