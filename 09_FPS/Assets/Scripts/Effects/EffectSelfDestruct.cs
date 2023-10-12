using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectSelfDestruct : MonoBehaviour
{
    VisualEffect effect;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();

        //ParticleSystem ps;
        //Destroy(this.gameObject, ps.main.duration);
        //ps.isPlaying

        int id = Shader.PropertyToID("Duration");
        float duration = effect.GetFloat(id);
        Destroy(this.gameObject, duration);
    }
}
