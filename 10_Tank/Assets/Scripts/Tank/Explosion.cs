using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : PooledObject
{
    VisualEffect vfx;
    readonly int OnExplosionID = Shader.PropertyToID("OnExplosion");

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }

    public void Initialize(Vector3 pos, Vector3 normal)
    {
        transform.position = pos;
        transform.up = normal;
        vfx.SendEvent(OnExplosionID);
        StartCoroutine(LifeOver(1.5f));
    }
}
