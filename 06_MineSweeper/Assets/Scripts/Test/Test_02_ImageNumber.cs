using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_ImageNumber : TestBase
{
    public int testNumber = 0;
    public ImageNumber imageNumber;

    private void OnValidate()
    {
        imageNumber.Number = testNumber;
    }

}
