using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.AnyKey.performed += Press;
        inputActions.UI.Click.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        gauge = FindObjectOfType<Slider>();
        loadingText = FindObjectOfType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();
        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        if(gauge.value < loadRatio)
        {
            gauge.value += (Time.deltaTime * loadingBarSpeed);
        }
    }

    private IEnumerator LoadingTextProgress()
    {
        float waitTime = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        string[] texts = { 
            "Loading", 
            "Loading .", 
            "Loading . .", 
            "Loading . . .", 
            "Loading . . . .", 
            "Loading . . . . ." };

        int index = 0;
        
        while(true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;

            yield return wait;
        }
    }

    IEnumerator LoadScene()
    {
        gauge.value = 0;
        loadRatio = 0;

        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;
            yield return null;
        }

        yield return new WaitForSeconds( (loadRatio - gauge.value) / loadingBarSpeed );

        StopCoroutine(loadingTextCoroutine);
        loadingDone = true;
        loadingText.text = "Loading\nComplete!";
    }

    private void Press(InputAction.CallbackContext _)
    {        
        if( loadingDone )
        {
            async.allowSceneActivation = true;
        }
    }
}
