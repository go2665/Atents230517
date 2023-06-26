using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    public float duration = 5.0f;
    ParticleSystem ps;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.Die();
        }
        StartCoroutine(StopEffect());
    }

    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }
}
