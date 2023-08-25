using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Network : TestBase
{
    public Vector3 newPos = new Vector3();

    protected override void Test1(InputAction.CallbackContext context)
    {
        NetPlayer netPlayer = FindObjectOfType<NetPlayer>();
        netPlayer.position.Value = newPos;
    }
}
