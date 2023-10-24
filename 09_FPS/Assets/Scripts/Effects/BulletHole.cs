using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletHole : PooledObject
{
    VisualEffect effect;

    float duration;
    readonly int DurationId = Shader.PropertyToID("Duration");
    readonly int PositionID = Shader.PropertyToID("SpawnPosition");
    readonly int ReflectID = Shader.PropertyToID("SpawnReflect");
    readonly int OnStartEvent = Shader.PropertyToID("OnStart");

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        duration = effect.GetFloat(DurationId);
    }

    private void OnEnable()
    {        
    }

    public void Initialize(Vector3 position, Vector3 normal, Vector3 reflect)
    {
        effect.SetVector3(PositionID, position);
        transform.position = position;
        transform.forward = -normal;

        effect.SetVector3(ReflectID, reflect);

        effect.SendEvent(OnStartEvent);
        StartCoroutine(LifeOver(duration));
    }
}

// 실습
// 1. 총알 구멍 이팩트용 오브젝트 풀 만들기
// 2. 팩토리 만들기