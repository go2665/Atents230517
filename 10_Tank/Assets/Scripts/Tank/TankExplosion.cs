using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TankExplosion : MonoBehaviour
{
    VisualEffect effect;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();        
    }

    private void Update()
    {
        if (!effect.HasAnySystemAwake())
        {
            Destroy(this.gameObject);
        }
    }
}
