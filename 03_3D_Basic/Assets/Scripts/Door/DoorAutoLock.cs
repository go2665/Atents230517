using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoLock : DoorAuto
{
    BoxCollider sensor;
    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<BoxCollider>();
        sensor.enabled = false;
    }

    public void UnLock()
    { 
        sensor.enabled = true; 
    }

}
