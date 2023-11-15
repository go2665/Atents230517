using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSubminuation : Shell
{
    public float angle = 15.0f;

    protected override void OnEnable()
    {
        base.OnEnable();

        Vector3 forward = Vector3.down;
        forward = Quaternion.Euler(Random.Range(-angle, angle), 0, 0) * forward;
        forward = Quaternion.Euler(0, Random.Range(0,360), 0) * forward;
        
        rigid.velocity = forward * firePower;
    }
}
