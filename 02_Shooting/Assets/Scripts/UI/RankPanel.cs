using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 패널에 붙어 있는 랭킹 줄들
    /// </summary>
    RankLine[] rankLines = null;

    /// <summary>
    /// 랭커의 이름을 입력받기 위한 인풋 필드
    /// </summary>
    TMP_InputField inputField;
       
    /// <summary>
    /// 최고 득점(1~5등)
    /// </summary>
    int[] highScores = null;

    /// <summary>
    /// 최고 득점자(1~5등)
    /// </summary>
    string[] rankerNames = null;

    /// <summary>
    /// 랭커가 추가로 들어온 인덱스
    /// </summary>
    int updatedIndex;

    /// <summary>
    /// 최대 랭크 갯수
    /// </summary>
    const int rankCount = 5;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>(true);  // 인풋 필드 찾기
        inputField.onEndEdit.AddListener(OnNameInputEnd);           // 이름 입력이 끝났을 때 실행될 함수등록
        rankLines = GetComponentsInChildren<RankLine>();    // 랭크 라인 찾기
        highScores = new int[rankCount];                    // 최고 점수용 배열생성
        rankerNames = new string[rankCount];                // 랭커의 이름 저장용 배열 생성
    }

    private void Start()
    {
        //SetDefaultData();
        LoadRankingData();  // 시작할 때 데이터 불러오기
    }

    /// <summary>
    /// 랭킹을 초기값으로 되돌리는 함수
    /// </summary>
    void SetDefaultData()
    {
        for(int i=0;i<rankCount;i++)
        {
            int score = 10;
            for(int j = rankCount-i; j>0 ;j--)
            {
                score *= 10;
            }
            highScores[i] = score;  //100만, 10만, 1만, 1천, 1백

            char temp = 'A';
            temp = (char)((byte)temp + i);
            rankerNames[i] = $"{temp}{temp}{temp}"; // 이름은 AAA~EEE까지

            rankLines[i].SetData(rankerNames[i], highScores[i]);// 라인즈에 랭커의 이름과 점수 세팅
        }
    }

    /// <summary>
    /// 파일을 저장하는 함수
    /// </summary>
    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);
        //int score = PlayerPrefs.GetInt("Score");

        // 저장할 데이터를 담는 클래스 만들고
        //SaveData saveData = new SaveData();
        SaveData saveData = new();  // 윗줄을 줄여서 쓴 것
        saveData.rankerNames = rankerNames; // 데이터 복사하고
        saveData.highScores = highScores;

        string json = JsonUtility.ToJson(saveData); // saveData를 json 형식의 문자열로 변경한 것

        // Directory : 폴더 관련 각종 편의기능을 제공하는 클래스
        // File : 파일 관련 각종 편의기능을 제공하는 클래스

        // Application.dataPath : 에디터에서는 Asssets, 빌드버전에서는 Data 폴더
        string path = $"{Application.dataPath}/Save/";
        if( !Directory.Exists(path))            // Exists : 특정 폴더가 있으면 true, 없으면 false
        {
            Directory.CreateDirectory(path);    // path에 지정된 폴더 만들기
        }

        string fullPath = $"{path}Save.json";
        File.WriteAllText(fullPath, json);      // json 변수에 들어있는 모든 텍스트를 fullPath에 저장하기
    }

    /// <summary>
    /// 랭킹 데이터를 불러오는 함수
    /// </summary>
    /// <returns>불러오기 성공 여부. 성공했으면 true, 아니면 false </returns>
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";  // 경로 구하기
        string fullPath = $"{path}Save.json";           // 전체 경로 구하기

        result = Directory.Exists(path) && File.Exists(fullPath);   // 폴더와 파일이 있는지 확인
        if( result )    // 둘 다 있을 때
        {
            // 로딩
            string json = File.ReadAllText(fullPath);   // 파일에 써있는 텍스트 모두 읽기

            // json형식으로 된 문자열을 파싱해서 SaveData형식으로 저장
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);   
            highScores = loadedData.highScores;
            rankerNames = loadedData.rankerNames;
        }
        else
        {
            // 디폴트값 세팅
            SetDefaultData();
        }

        RefreshRankLines(); // 다 불러왔으면 화면 갱신

        return result;
    }

    /// <summary>
    /// 현재 설정된 데이터 값에 맞게 UI 갱신
    /// </summary>
    void RefreshRankLines()
    {
        for(int i = 0; i < rankCount; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

    /// <summary>
    /// 랭킹 업데이트 하는 함수
    /// </summary>
    /// <param name="score">새 점수</param>
    void RankUpdate(int score)
    {
        for(int i=0;i<rankCount; i++)   // 1등 -> 5등까지
        {
            if (highScores[i] < score)  // 각 등수가 score보다 작으면
            {
                for(int j = rankCount-1; j > i; j--)        // 아래로 한칸씩 밀기
                {
                    highScores[j] = highScores[j - 1];
                    rankerNames[j] = rankerNames[j - 1];
                }
                highScores[i] = score;
                //rankerNames[i] = "akakakak";
                updatedIndex = i;       // 밀리기 시작한 위치 기록

                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                inputField.transform.position = newPos;     // 인풋 필드의 위치 설정
                inputField.gameObject.SetActive(true);      // 인풋 필드 보이게 만들기
                break;
            }
        }
    }

    /// <summary>
    /// 인풋 필드의 입력이 끝났을 때 호출되는 함수
    /// </summary>
    /// <param name="text">최종적으로 입력된 값</param>
    private void OnNameInputEnd(string text)
    {
        rankerNames[updatedIndex] = text;       // 이름 변경
        inputField.gameObject.SetActive(false); // 인풋 필드 안보이게 만들기
        RefreshRankLines();                     // 화면 갱신하고
        SaveRankingData();                      // 저장하기
    }

    public void TestSave()
    {
        SaveRankingData();
    }

    public void TestLoad()
    {
        LoadRankingData();
    }

    public void TestRankUpdate(int score)
    {
        RankUpdate(score);        
    }

    // 코드 확인
    // 플레이어가 죽었을 때 RankUpdate가 실행되도록 수정하기
}
