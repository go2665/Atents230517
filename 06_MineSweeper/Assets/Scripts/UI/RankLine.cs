using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI record;
    TextMeshProUGUI countWord;
    TextMeshProUGUI rankerName;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        rank = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        record = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        countWord = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        rankerName = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// RankLine에 데이터 세팅해서 보이게 만드는 함수
    /// </summary>
    /// <typeparam name="T">등수 기록에 대한 데이터 타입(int, float만 사용)</typeparam>
    /// <param name="rankData">이 기록의 등수</param>
    /// <param name="recordData">기록</param>
    /// <param name="nameData">기록을 달성한 사람의 이름</param>
    public void SetData<T>(int rankData, T recordData, string nameData )
    {
        rank.text = $"{rankData}등";
        if (recordData.GetType() == typeof(int))
        {
            record.text = recordData.ToString();
        }
        else if(recordData.GetType() == typeof(float))
        {
            record.text = $"{recordData:f1}";
        }
        rankerName.text = nameData;
        countWord.enabled = true;
    }

    /// <summary>
    /// RankLine을 안보이게 만드는 함수
    /// </summary>
    public void ClearLine()
    {
        rank.text = string.Empty;
        record.text = string.Empty;
        rankerName.text = string.Empty;
        countWord.enabled = false;
    }

    /// <summary>
    /// 개수를 나타내는 말을 변경하는 함수
    /// </summary>
    /// <param name="str">"회"나 "초" 사용</param>
    public void SetCountWord(string str)
    {
        countWord.text = str;
    }
}
