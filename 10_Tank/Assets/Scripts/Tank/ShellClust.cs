using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellClust : Shell
{

    // 2. 클러스트 탄
    //  2.1. 발사하면 하늘로 올라감
    //  2.2. 일정이상 올라가면 자폭하면서 자식 포탄을 30개 정도 아래로 쏱아냄

    public float upPower = 20.0f;
    public float guideHeight = 10.0f;
    public int subCount = 30;

    private void FixedUpdate()
    {
        if (!isExplosion && transform.position.y < guideHeight)
        {
            rigid.AddForce(Vector3.up * upPower);
            rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));
        }
        else
        {
            Explosion(transform.position, Vector3.up);
        }
    }

    protected override void OnExplosion()
    {
        rigid.AddForce(Vector3.down * 20, ForceMode.Impulse);
        // 자식포탄 쏱아내기

        for(int i = 0; i < subCount; i++)
        {
            Factory.Inst.GetSubminuation(transform.position);
        }
    }
}
