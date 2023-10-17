using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Test_07_Shoot : TestBase
{
    public VisualEffect effect;
    public TestBulletHole testBulletHole;
    public Transform bulletHoleTransform;

    public GunBase gunBase;

    protected override void Test1(InputAction.CallbackContext context)
    {
        int id = Shader.PropertyToID("OnFire");
        effect.SendEvent(id);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        testBulletHole.Initialize(bulletHoleTransform.position, bulletHoleTransform.up);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        StartCoroutine(gunBase.TestFire(gunBase.clipSize));

    }
}
