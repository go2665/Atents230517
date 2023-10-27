using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    public int pellet = 6;

    protected override void FireProcess(bool isFireStart = true)
    {
        if (isFireStart)
        {            
            base.FireProcess(isFireStart);

            for (int i=0;i<pellet; i++)
            {
                ShotProcess();
            }

            FireRecoil();
        }
    }
}
