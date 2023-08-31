using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProvess_고병조 : MonoBehaviour
{
    Volume volume;
    Vignette vignette;
    float direction;
    float speed;

    private void Start()
    {
        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
        vignette.rounded.value = true;

        direction = 1;  // intensity가 증가하는지 감소하는지 결정(1 아니면 -1)
        speed = 0.5f;   // intensity의 변화 정도를 결정
    }

    private void Update()
    {
        vignette.intensity.value += Time.deltaTime * direction * speed;
        if(vignette.intensity.value >= 1 || vignette.intensity.value <= 0)
        {
            direction *= -1.0f;
        }
    }
}
