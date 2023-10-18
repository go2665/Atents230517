using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class Test_08_HTTP : TestBase
{
    readonly string url = "https://atentsexample.azurewebsites.net/monster";
    //readonly string url2 = "https://www.google.com";

    protected override void Test1(InputAction.CallbackContext context)
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url); // url에 있는 데이터를 가져오는 HTTP 요청 만들기
        yield return www.SendWebRequest();              // 요청을 보내고 돌아 올때까지 대기

        if( www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
        }
    }
}
