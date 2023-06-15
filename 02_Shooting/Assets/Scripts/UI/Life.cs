using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
    // 플레이어의 HP가 변경되면 UI에서 변경해서 보여주기
    TextMeshProUGUI lifeText;

    private void Awake()
    {
        Transform textTransform = transform.GetChild(2);
        lifeText = textTransform.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // 플레이어의 onLifeChange가 발동되면 실행할 람다식 연결
        GameManager.Inst.Player.onLifeChange += (life) => lifeText.text = life.ToString();
    }
}
