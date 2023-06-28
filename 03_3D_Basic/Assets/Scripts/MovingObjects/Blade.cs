using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WaypointUser
{
    protected override Transform Target 
    { 
        get => base.Target; 
        set => base.Target = value; 
    }
}

// 1. 이동 방향을 바라보아야 한다.
// 2. 날 부분이 돌아가야 한다.
// 3. 플레이어가 닿으면 플레이어가 죽어야 한다.