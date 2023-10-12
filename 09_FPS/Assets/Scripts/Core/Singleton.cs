using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private bool initialized = false;

    /// <summary>
    /// 이미 종료처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤의 객체.
    /// </summary>
    private static T instance;

    /// <summary>
    /// 싱글톤의 객체를 읽기 위한 프로퍼티. 객체가 만들어지지 않았으면 새로 만든다.
    /// </summary>
    public static T Inst
    {
        get
        {
            if (isShutDown)      // 종료처리에 들어간 상황이면
            {
                Debug.LogWarning($"{typeof(T).Name} 싱글톤은 이미 삭제 중이다.");    // 경고 출력하고
                return null;    // null 리턴
            }
            if (instance == null)
            {
                // instance가 없으면 새로 만든다.

                T sigleton = FindObjectOfType<T>();         // 씬에서 싱글톤 찾아보기
                if (sigleton == null)                       // 씬에 싱글톤이 있는지 확인
                {
                    // 씬에도 싱글톤이 없다.
                    GameObject gameObj = new GameObject();  // 빈오브젝트 만들고
                    gameObj.name = $"{typeof(T).Name} Singleton";   // 이름 수정하고
                    sigleton = gameObj.AddComponent<T>();   // 싱글톤 컴포넌트 추가  
                }
                instance = sigleton;                        // instance에 찾았거나 만들어진 객체 대입
                DontDestroyOnLoad(instance.gameObject);     // 씬이 사라질 때 게임오브젝트가 삭제되지 않도록 설정
            }

            return instance;    // instance리턴(이미 있거나 새로 만들었다.)
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // 씬에 배치되어 있는 첫번째 싱글톤 게임오브젝트
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            // 첫번째 싱글톤 게임 오브젝트가 만들어진 이후에 만들어진 싱글톤 게임 오브젝트
            if (instance != this)
            {
                Destroy(this.gameObject);   // 첫번째 싱글톤과 다른 게임 오브젝트면 삭제
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if( !initialized )
        {
            OnPreInitialize();
        }
        if( mode != LoadSceneMode.Additive )    // 그냥 자동으로 씬로딩될 때는 4번이 들어옴
        {            
            OnInitialize();
        }
    }

    /// <summary>
    /// 프로그램이 종료될 때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;  // 종료 표시
    }

    /// <summary>
    /// 싱글톤이 만들어질 때 단 한번만 호출될 초기화 함수
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        initialized = true;
    }

    /// <summary>
    /// 싱글톤이 만들어지고 씬이 Single로 로드될 때마다 호출될 초기화 함수
    /// </summary>
    protected virtual void OnInitialize()
    {
    }


}
