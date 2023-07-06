using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeShader : TestBase
{
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

    // 실습
    // 1. innerLine은 처음 시작할 때 안보이다가 시간이 흐름에 따라 점점 두꺼워진다.
    //    전체가 다차면 다시 점점 얇아지다가 안보이게 된다. 그리고 계속 반복
    // 2. Phase, PhaseReverse, Dissolve 핑퐁시키기   
    // 3. Outline, Phase, Dissolve를 한번에 하는 쉐이더 그래프 만들기
}
