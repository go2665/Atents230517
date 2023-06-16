using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI recordText;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        nameText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        recordText = child.GetComponent<TextMeshProUGUI>();
    }

    public void SetData(string rankerName, int record)
    {
        nameText.text = rankerName;
        recordText.text = record.ToString("N0");    // N0로 해야 세자리마다 콤마가 찍힌다.
    }

}
