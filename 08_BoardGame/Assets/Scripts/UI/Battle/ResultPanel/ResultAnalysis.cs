using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultAnalysis : MonoBehaviour
{
    public int AllAttackCount
    {
        set
        {
            texts[0].text = $"{value} 회";
        }
    }

    public int SuccessAttackCount
    {
        set
        {
            texts[1].text = $"{value} 회";
        }
    }

    public int FailAttackCount
    {
        set
        {
            texts[2].text = $"{value} 회";
        }
    }

    public float SuccessAttackRate
    {
        set
        {
            texts[3].text = $"{value * 100.0f:f1} %";
        }
    }


    TextMeshProUGUI[] texts;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        texts = child.GetComponentsInChildren<TextMeshProUGUI>();
    }
}
