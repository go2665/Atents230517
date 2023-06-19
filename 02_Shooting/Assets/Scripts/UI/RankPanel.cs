using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;

    int[] highScores = null;
    string[] rankerNames = null;

    const int rankCount = 5;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScores = new int[rankCount];
        rankerNames = new string[rankCount];
    }

    private void Start()
    {
        SetDefaultData();
    }

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
            rankerNames[i] = $"{temp}{temp}{temp}";

            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

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

    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";

        result = Directory.Exists(path) && File.Exists(fullPath);
        if( result )
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

        RefreshRankLines();

        return result;
    }

    void RefreshRankLines()
    {
        for(int i = 0; i < rankCount; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

    public void TestSave()
    {
        SaveRankingData();
    }

    public void TestLoad()
    {
        LoadRankingData();
    }
}
