using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : GunBase
{
    // 연사 가능 (공격 버튼을 누르고 있으면 계속 발사, 때면 정지)
    protected override void FireProcess(bool isFireStart)
    {
        if(isFireStart)
        {
            // 발사 시작
            StartCoroutine(FireRepeat());
        }
        else
        {
            // 발사 종료
            StopAllCoroutines();
            isFireReady = true;
        }
    }

    IEnumerator FireRepeat()
    {
        while(BulletCount > 0) 
        {
            MuzzleEffect();
            BulletCount--;

            ShotProcess();

            FireRecoil();

            yield return new WaitForSeconds(1/fireRate);
        }

        isFireReady = true;
    }
}
