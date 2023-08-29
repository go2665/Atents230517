using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Test_04_Chat : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        NetworkObject obj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        GameManager.Inst.Log(obj.gameObject.name);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        // commandLine이 "/setname AAAbb가" 이렇게 입력되었을 경우
        // GameManager.userName은 "AAAbb가"가 되어야 한다.
        string input = "/SetName AAAbb가";
        int space = input.IndexOf(' ');
        string commandToken = input.Substring(0, space);
        commandToken = commandToken.ToLower();
        string parameterToken = input.Substring(space + 1);

        GameManager.Inst.UserName = parameterToken;
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        // commandLine이 "/setcolor 1, 0, 0" 이렇게 입력되었을 경우
        // 플레이어의 색상은 빨간색으로 변경되어야 한다.(접속해 있을 때만)

        string commandLine = "/setcolor 1.5, 0.0f, -0.5, 0.7, 7";
        int space = commandLine.IndexOf(' ');
        string commandToken = commandLine.Substring(0, space);
        commandToken = commandToken.ToLower();
        string parameterToken = commandLine.Substring(space + 1);

        string[] splitNumbers = parameterToken.Split(',', ' ');

        float[] colorValues = new float[4] { 0, 0, 0, 0}; // r,g,b,a 순으로 들어간다.
        int count = 0;
        foreach (string number in splitNumbers)
        {
            if (number.Length == 0)
                continue;

            if(count > 3)
            {
                break;
            }

            if( !float.TryParse(number, out colorValues[count]) ) // 일단 변경 시도하고
            {
                colorValues[count] = 0; // 실패하면 0
            }
            count++;
        }

        for(int i=0;i<colorValues.Length;i++)
        {
            colorValues[i] = Mathf.Clamp01(colorValues[i]);
        }

        Color color = new Color(colorValues[0], colorValues[1], colorValues[2], colorValues[3]);
        GameManager.Inst.Log($"플레이어의 색상을 변경했습니다 : ({color.r},{color.g},{color.b},{color.a})");
    }
}
