using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    // Player가 보유한 Money가 변경되면 이 UI에서 보여지는 금액이 변경된다.
    // 금액은 세자리마다 ,를 표시한다.
    TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Refresh(int money)
    {
        moneyText.text = $"{money:N0}";
    }
}
