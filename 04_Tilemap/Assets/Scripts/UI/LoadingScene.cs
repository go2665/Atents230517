using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// 다음에 로딩할 씬의 이름
    /// </summary>
    public string nextSceneName = "12_Test_AsyncSample";

    /// <summary>
    /// 비동기 명령 처리용
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// 로딩바의 value가 목표로 하는 값(0~1)
    /// </summary>
    float loadRatio;

    /// <summary>
    /// 로딩바가 증가하는 속도
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// LoadingText변화용 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// 로딩이 완료(게이지가 끝까지 다 찼을 때가 완료)되었다고 표시하는 변수
    /// </summary>
    bool loadingDone = false;

    Slider gauge;
    TextMeshProUGUI loadingText;
    PlayerInputActions inputActions;

    //실습
    // 0. nextSceneName의 이름을 가진씬을 비동기로딩 시작한다.
    // 1. loadingText가 반복해서 변경된다.
    //   "Loading", "Loading .", "Loading . .", "Loading . . .", "Loading . . . .", "Loading . . . . ."
    // 2. 로딩이 완료되면 loadingText가 "Loading Complete!"로 변경된다.(단 Complete는 줄바꿈을 하고 출력된다.)
    // 3. gauge의 value는 loadRatio를 목표로 loadingBarSpeed로 증가한다.
    // 4. 로딩이 완료된 이후에(gauge의 value가 1이 된 이후) 키를 입력하거나 클릭이 일어나면 nextSceneName으로 전환된다.
}
