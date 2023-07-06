using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeShader : TestBase
{
    /// <summary>
    /// 변화 속도
    /// </summary>
    public float speed = 0.5f;

    /// <summary>
    /// 시간 누적용
    /// </summary>
    float timeElapsed = 0.0f;

    readonly int ThicknessID = Shader.PropertyToID("_Thickness");
    readonly int SpliteID = Shader.PropertyToID("_Split");
    readonly int FadeID = Shader.PropertyToID("_Fade");


    enum ShaderType
    {
        OutLine = 0,
        innerLine,
        Phase,
        PhaseReverse,
        Dissolve
    }

    public Renderer[] renderers;
    Material[] materials;

    private void Start()
    {
        materials = new Material[renderers.Length];
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = renderers[i].material;
            //materials[i] = renderers[i].sharedMaterial;
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f; // cos함수 이용해서 0~1로 숫자 핑퐁되게 만들기

        Material inner = GetMaterial(ShaderType.innerLine);
        inner.SetFloat(ThicknessID, num * 0.03f);   // 두께는 최대치가 0.03이기 때문에 추가로 곱함
        Material phase = GetMaterial(ShaderType.Phase);
        phase.SetFloat(SpliteID, num);
        Material phaseRev = GetMaterial(ShaderType.PhaseReverse);
        phaseRev.SetFloat(SpliteID, num);
        Material dissolve = GetMaterial(ShaderType.Dissolve);
        dissolve.SetFloat(FadeID, num);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("_Thickness");
        Material mat = GetMaterial(ShaderType.OutLine);
        mat.SetFloat(id, 0);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("_Thickness");
        Material mat = GetMaterial(ShaderType.OutLine);
        mat.SetFloat(id, 0.004f);
    }

    Material GetMaterial(ShaderType type)
    {
        Material material = null;
        switch(type)
        {
            case ShaderType.OutLine:
                material = materials[0];
                break;
            case ShaderType.innerLine: 
                material = materials[1];
                break;
            case ShaderType.Phase:
                material = materials[2];
                break;
            case ShaderType.PhaseReverse:
                material = materials[3];
                break;
            case ShaderType.Dissolve:
                material = materials[4];
                break;
        }

        return material;
    }
}
