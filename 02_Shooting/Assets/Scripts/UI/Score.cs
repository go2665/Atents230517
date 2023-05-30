using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI scoreUI;

    private void Awake()
    {
        scoreUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        scoreUI.text = "Test Text";
    }
}

// 델리게이트를 이용해서 Player의 Score가 변경되면 scoreUI의 점수를 변경하는 코드를 작성하기
// scoreUI에서 점수를 출력하는 양식은 "Score : 123"
