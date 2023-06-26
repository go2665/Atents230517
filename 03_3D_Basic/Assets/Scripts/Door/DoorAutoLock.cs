using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoLock : DoorAuto
{
    public Color lockColor;
    public Color unlockColor;

    BoxCollider sensor;
    Material doorMaterial;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<BoxCollider>();
        sensor.enabled = false;

        Transform door = transform.GetChild(1);
        door = door.GetChild(0);
        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>();
        doorMaterial = meshRenderer.material;
    }

    private void Start()
    {
        doorMaterial.color = lockColor;        
    }

    public void UnLock()
    { 
        sensor.enabled = true;
        doorMaterial.color = unlockColor;
    }

}
