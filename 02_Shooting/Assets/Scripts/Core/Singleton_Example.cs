using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤 : 객체를 하나만 가지는 디자인 패턴.
/// </summary>
public class Singleton_Example : MonoBehaviour
{
    /// <summary>
    /// 이미 종료처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤의 객체.
    /// </summary>
    private static Singleton_Example instance;

    /// <summary>
    /// 싱글톤의 객체를 읽기 위한 프로퍼티. 객체가 만들어지지 않았으면 새로 만든다.
    /// </summary>
    public static Singleton_Example Instance
    {
        get
        {
            if(isShutDown)      // 종료처리에 들어간 상황이면
            {
                Debug.LogWarning("싱글톤은 이미 삭제 중이다.");    // 경고 출력하고
                return null;    // null 리턴
            }
            if( instance == null)
            {
                // instance가 없으면 새로 만든다.

                Singleton_Example sigleton = FindObjectOfType<Singleton_Example>(); // 씬에서 싱글톤 찾아보기
                if (sigleton == null)   // 씬에 싱글톤이 있는지 확인
                {
                    // 씬에도 싱글톤이 없다.
                    GameObject gameObj = new GameObject();                  // 빈오브젝트 만들고
                    gameObj.name = "Singleton";                             // 이름 수정하고
                    sigleton = gameObj.AddComponent<Singleton_Example>();   // 싱글톤 컴포넌트 추가  
                }
                instance = sigleton;    // instance에 찾았거나 만들어진 객체 대입
                DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 게임오브젝트가 삭제되지 않도록 설정
            }
            
            return instance;    // instance리턴(이미 있거나 새로 만들었다.)
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            // 씬에 배치되어 있는 첫번째 싱글톤 게임오브젝트
            instance = this;
            DontDestroyOnLoad(instance.gameObject); 
        }
        else
        {
            // 첫번째 싱글톤 게임 오브젝트가 만들어진 이후에 만들어진 싱글톤 게임 오브젝트
            if( instance != this)
            {
                Destroy(this.gameObject);   // 첫번째 싱글톤과 다른 게임 오브젝트면 삭제
            }
        }
    }

    /// <summary>
    /// 프로그램이 종료될 때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;  // 종료 표시
    }
}


/// <summary>
/// 일반 싱글톤 예제
/// </summary>
public class TestSingleton
{
    // static변수를 만들어서 객체를 만들지 않고 사용할 수 있게 만들기
    private static TestSingleton instance = null;

    // 다른 곳에서 instance를 수정하지 못하도록 읽기 전용 프로퍼티 만들기
    public static TestSingleton Instance
    {
        get
        {
            if (instance == null)   // 처음 접근했을 때 new하기.
            {
                instance = new TestSingleton();
            }
            return instance;        // 항상 return될 때 값은 존재한다.
        }
    }

    // 중복생성 방지 목적. private으로 생성자를 만들어 기본 public생성자가 생성되지 않게 막기
    private TestSingleton()
    {
    }
}
