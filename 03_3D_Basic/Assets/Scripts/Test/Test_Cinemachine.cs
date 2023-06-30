using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Cinemachine : TestBase
{
    public CinemachineVirtualCamera[] vcams;

    private void Start()
    {
        if(vcams == null)
        {
            vcams = FindObjectsOfType<CinemachineVirtualCamera>();
        }
        ResetVCamPriority();
    }

    void ResetVCamPriority()
    {
        foreach(var vcam in vcams)
        {
            vcam.Priority = 10;
        }
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        ResetVCamPriority();
        vcams[0].Priority = 100;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        ResetVCamPriority();
        vcams[1].Priority = 100;
    }
}
