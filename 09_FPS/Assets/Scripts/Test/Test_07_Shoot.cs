using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Test_07_Shoot : TestBase
{
    public VisualEffect effect;

    protected override void Test1(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("OnFire");
        effect.SendEvent(id);
    }
}
