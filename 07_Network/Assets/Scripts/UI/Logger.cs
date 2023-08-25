using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    /// <summary>
    /// 로거에서 한번에 표시 가능한 최대 줄 수
    /// </summary>
    const int MaxLineCount = 20;

    /// <summary>
    /// 문자열을 합치기 위한 StringBuilder의 인스턴스
    /// </summary>
    StringBuilder sb;

    // 글자출력용 컴포넌트
    TextMeshProUGUI log;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();

        sb = new StringBuilder(MaxLineCount);
    }

    private void Start()
    {
        Clear();
    }

    /// <summary>
    /// 로거에 문장을 추가하는 함수
    /// </summary>
    /// <param name="logText">추가할 문장</param>
    public void Log(string logText)
    {
        // logText = "[위험]합니다. {경고}입니다.";
        // 위험이라는 글자는 빨간색으로 출력이 된다.
        // 경고라는 글자는 노란색으로 출력이 된다.
        // 괄호는 반드시 열리면 닫혀야 한다.(중복으로 열린 것은 처리하지 않는다)


        //sb.Append(logText);
        //sb.ToString();
    }

    /// <summary>
    /// 로거에 표시되는 글자를 모두 비우는 함수
    /// </summary>
    void Clear()
    {
        log.text = "";
    }
}
