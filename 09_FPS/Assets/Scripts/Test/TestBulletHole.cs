using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestBulletHole : MonoBehaviour
{
    VisualEffect effect;

    float duration;
    readonly int DurationId = Shader.PropertyToID("Duration");
    readonly int PositionID = Shader.PropertyToID("SpawnPosition");
    readonly int NormalID = Shader.PropertyToID("SpawnNormal");
    readonly int OnStartEvent = Shader.PropertyToID("OnStart");

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        duration = effect.GetFloat(DurationId);
    }

    public void Initialize(Vector3 position, Vector3 normal)
    {
        effect.SetVector3(PositionID, position);
        transform.position = position;
        transform.forward = -normal;

        effect.SetVector3(NormalID, normal);

        effect.SendEvent(OnStartEvent);
    }
}