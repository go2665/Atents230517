using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadTime = 1.0f;
    bool isReloading = false;

    protected override void FireProcess(bool isFireStart)
    {
        base.FireProcess(isFireStart);

        Ray ray = new(fireTransform.position, GetFireDirection());
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range) )
        {
            Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
            Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal, reflect);
            //bulletHole.transform.position = hitInfo.point;
            //bulletHole.transform.forward = -hitInfo.normal;
        }

        FireRecoil();
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
