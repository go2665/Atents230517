using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    protected override void FireProcess()
    {
        Ray ray = new(fireTransform.position, fireTransform.forward);
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range) )
        {
            Instantiate(GameManager.Inst.bulletHolePrefab, hitInfo.point, Quaternion.identity);
        }
    }
}
