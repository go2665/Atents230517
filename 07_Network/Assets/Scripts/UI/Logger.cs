using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Logger : MonoBehaviour
{
    public Color warningColor;
    public Color errorColor;

    /// <summary>
    /// 로거에서 한번에 표시 가능한 최대 줄 수
    /// </summary>
    const int MaxLineCount = 20;

    /// <summary>
    /// 문자열을 합치기 위한 StringBuilder의 인스턴스
    /// </summary>
    StringBuilder sb;

    /// <summary>
    /// 로그창에 출력될 모든 문자열 저장용 큐
    /// </summary>
    Queue<string> logLines;

    // 글자출력용 컴포넌트
    TextMeshProUGUI log;
    TMP_InputField inputField;

    private void Awake()
    {
        Transform child = transform.GetChild(0).GetChild(0).GetChild(0);
        log = child.GetComponent<TextMeshProUGUI>();
        inputField = GetComponentInChildren<TMP_InputField>();

        // onEndEdit 이벤트(입력이 끝났을 때 실행, 포커스를 옮기는 것으로도 실행이 된다.)
        // onSubmit 이벤트(입력이 완료되었을 때 실행, 비어있을 때나 focus 옮기는 것으로는 발동되지 않는다.)
        inputField.onSubmit.AddListener((text) =>
        {
            if(GameManager.Inst.Player != null)
            {
                // 접속해 있는 상황이면 
                GameManager.Inst.Player.SendChat(text); // 채팅으로 보내기
            }
            else
            {
                // 접속해 있지 않은 상황이면
                Log(text);  // 로거에 글자 찍기
            }

            inputField.text = string.Empty;     // 인풋필드의 입력창 비우고
            inputField.ActivateInputField();    // 포커스 다시 활성화(무조건 활성화)
            //inputField.Select();  // 활성화 되어있을 떄는 비활성화, 비활성화 되어있을 때는 활성화
        });

        logLines = new Queue<string>(MaxLineCount + 5);
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
        logText = Emphasize(logText, '[', ']', errorColor);     // 괄호 종류에 맞게 색으로 강조
        logText = Emphasize(logText, '{', '}', warningColor);

        logLines.Enqueue(logText);          // 한줄 추가하고
        if(logLines.Count > MaxLineCount )  // 줄 개수가 넘어서면
        {
            logLines.Dequeue();             // 첫번째 줄 제거
        }
        sb.Clear();                         // 스트링 빌더 초기화하고 새로 조립
        foreach( string line in logLines )
        {
            sb.AppendLine( line );          // 큐에 들어있는 문자열을 순서대로 스트링 빌더에 추가
        }        
        log.text = sb.ToString();           // 다 합쳐진 것을 텍스트로 출력
    }

    /// <summary>
    /// 원문에 있는 괄호 사이에 있는 글자의 색상을 변경해서 강조하는 함수
    /// </summary>
    /// <param name="source">원문</param>
    /// <param name="open">여는 괄호</param>
    /// <param name="close">닫는 괄호</param>
    /// <param name="color">괄호 안의 문장의 색상</param>
    /// <returns>강조 처리가 끝난 문자열</returns>
    string Emphasize(string source, char open, char close, Color color)
    {
        string result = source;
        if (IsPair(source, open, close))        // 이 문자열에서 괄호가 쌍으로 열리고 닫혔는지 확인
        {
            string[] split = result.Split(open, close);             // 괄호를 기준으로 문자열 나누기

            string colorText = ColorUtility.ToHtmlStringRGB(color); // Color 구조체의 내용을 16진수 RGB문자열로 변경

            result = string.Empty;              // result는 우선 비우기
            for(int i=0;i<split.Length; i++)    // 잘려진 토큰 단위로 처리
            {
                result += split[i];             // 우선 토큰 하나 result에 추가
                if(i != split.Length - 1)       // 마지막이 아닐 때(=괄호처리)
                {
                    if ( i % 2 == 0 )                   // i가 짝수면 그 뒤에는 괄호가 열렸을 것
                    {
                        result += $"<#{colorText}>";    // 컬러 태그의 시작부분 추가
                    }
                    else
                    {                                   // i가 홀수면 그 뒤에는 괄로가 닫혔을 것
                        result += "</color>";           // 컬러 태그의 끝부분 추가
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 원문에 지정된 괄호가 정확한 조건으로 들어있는지 체크하는 함수
    /// </summary>
    /// <param name="source">확인할 원문</param>
    /// <param name="open">여는 괄호</param>
    /// <param name="close">닫는 괄호</param>
    /// <returns>true면 정확하게 조건을 지키고 있다. false면 잘못되었다.</returns>
    bool IsPair(string source, char open, char close)
    {
        // 정확한 괄호의 조건
        // 1. 열리면 닫혀야 한다.
        // 2. 연속해서 여는 것은 안된다.

        bool result = true;

        int count = 0;      // 원문을 탐색할 때 발견된 괄호의 수.(괄호가 열리거나 닫힐 때마다 1씩 증가)
        for(int i=0;i<source.Length;i++)                    // 원문 전체 탐색하기
        {
            if( source[i] == open || source[i] == close )   // 해당 번째의 글자가 괄호라면
            {
                count++;                        // count 1 증가
                if(count % 2 == 1)              // count가 홀수이면 열릴 타이밍
                {
                    if (source[i] != open)      // 열릴 타이밍에 열리지 않았으면
                    {
                        result = false;         // 실패로 종료
                        break;
                    }    
                }
                else                            // count가 짝수이면 닫힐 타이밍
                {
                    if(source[i] != close)      // 닫힐 타이밍에 닫히지 않았으면
                    {
                        result = false;         // 실패로 종료
                        break;
                    }
                }
            }
        }

        // 괄호가 하나도 없는 경우는 실패. count == 0
        // 괄호가 열리기만 하고 닫히지 않았으면 실패. count % 2 != 0
        if (count == 0 || count % 2 != 0)     
        {
            result = false;
        }

        return result;
    }

    /// <summary>
    /// 로거에 표시되는 글자를 모두 비우는 함수
    /// </summary>
    public void Clear()
    {
        log.text = "";
    }

    /// 테스트용 --------------------------------------------------------------------------------------
    public bool TestIsPair(string source, char open, char close)
    {
        return IsPair(source, open, close);
    }
}
