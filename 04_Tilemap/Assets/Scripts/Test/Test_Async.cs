using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Async : TestBase
{
    // 비동기 명령어의 여러 정보를 가지는 클래스
    AsyncOperation async;

    protected override void Test1(InputAction.CallbackContext context)
    {
        // SceneManager.LoadScene(1); // 동기(Synchronous) 방식의 씬 로딩        

        StartCoroutine(LoadScene());
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;      // true가 되는 순간 맵 전환이 일어남
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;     // 비동기로 불러오는 씬을 열지 말 것

        while(async.progress < 0.9f)    // progress의 범위 : 0 ~ 0.9 사이. 로딩이 완료되면 0.9
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }
        Debug.Log("Loading Complete");
    }
}
