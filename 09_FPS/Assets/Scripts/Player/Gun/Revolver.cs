using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    protected override void FireProcess()
    {
        Ray ray = new(fireTransform.position, GetFireDirection());
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range) )
        {
            Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal);
            //bulletHole.transform.position = hitInfo.point;
            //bulletHole.transform.forward = -hitInfo.normal;
        }

        FireRecoil();
    }
}
