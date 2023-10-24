using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class Ranking_231024 : MonoBehaviour
{
    readonly string url = "https://atentsexample.azurewebsites.net/rankdata";
    RankLine[] rankLines;
    Button rankGetButton;

    private void Start()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        GameObject obj = GameObject.Find("RankGetButton");
        rankGetButton = obj.GetComponent<Button>();
        rankGetButton.onClick.AddListener(OnRankGetClick);
    }

    private void OnRankGetClick()
    {
        StartCoroutine(GetRank());
    }

    IEnumerator GetRank()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            
            if ( www.result != UnityWebRequest.Result.Success )
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                RankData data = JsonUtility.FromJson<RankData>(json);

                for (int i=0;i< rankLines.Length; i++)
                {
                    rankLines[i].SetRankerName(data.rankerName[i]);
                    rankLines[i].SetHighScore(data.highScore[i]);
                }
            }
        }
    }
}
