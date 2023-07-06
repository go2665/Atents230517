using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // 등장시
    //  - 아웃라인의 두께는 0
    //  - Dissolve의 fade는 1
    //  - Phase의 split은 1
    // 등장 직후에 Phase의 split이 1 -> 0로 변화한다.
    // 플레이어에게 공격 당할 수 있는 상태가 되면 두께는 0.005가 된다.
    // 죽으면 Dislove의 fade가 1 -> 0로 변화한다.

    /// <summary>
    /// 페이즈가 진행되는 시간
    /// </summary>
    public float phaseDuration = 0.5f;

    SpriteRenderer spriteRenderer;
    Material mainMaterial;

    const float VisibleOutlineThickness = 0.005f;
    const float VisiblePhaseThickness = 0.1f;

    readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplit = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int DissolveFade = Shader.PropertyToID("_DissolveFade");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;
    }

    void ResetShaderProperty()
    {
        mainMaterial.SetFloat(OutlineThickness, 0.0f);
        mainMaterial.SetFloat(PhaseSplit, 1.0f);
        mainMaterial.SetFloat(PhaseThickness, VisiblePhaseThickness);
        mainMaterial.SetFloat(DissolveFade, 1.0f);
    }

    IEnumerator StartPhase()
    {
        float timeElapsed = 0.0f;
        float phaseNormalize = 1.0f / phaseDuration;

        while (timeElapsed < phaseDuration)
        {
            timeElapsed += Time.deltaTime;
            mainMaterial.SetFloat(PhaseSplit, 1 - (timeElapsed * phaseNormalize));  // 나누기를 자주하는 것을 피하기 위해 미리 계산해 놓은 phaseNormailze 사용
            yield return null;
        }
    }

    public void ShowOutline(bool isShow = true)
    {
        if(isShow)
        {
            mainMaterial.SetFloat(OutlineThickness, VisibleOutlineThickness);
        }
        else
        {
            mainMaterial.SetFloat(OutlineThickness, 0.0f);
        }
    }
}
