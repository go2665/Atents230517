using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
    public ItemCode itemCode;
    public bool isNoise = true;
    public uint count = 3;
    Transform testTransform;

    private void Start()
    {
        testTransform = transform.GetChild(0);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItem(itemCode);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItem(itemCode, testTransform.position, isNoise);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItems(itemCode, count);
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItems(itemCode, count, testTransform.position, isNoise);
    }
}
