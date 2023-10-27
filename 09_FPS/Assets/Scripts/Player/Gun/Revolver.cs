using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadTime = 1.0f;
    bool isReloading = false;

    protected override void FireProcess(bool isFireStart)
    {
        if(isFireStart)
        {
            base.FireProcess(isFireStart);

            ShotProcess();
            FireRecoil();
        }
    }

    public void Reload()
    {
        if(!isReloading)
        {
            isReloading = true;
            isFireReady = false;
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds( reloadTime );
        isFireReady = true;
        BulletCount = clipSize;
        isReloading = false;
    }
}
