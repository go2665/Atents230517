using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
조건1 : TempPlayer_이름.cs 파일의 파일과 클래스 이름의 "이름"부분을 자신의 이름으로 변경한다.

조건2 : 현재 HP(float health)와 최대 HP(float maxHealth)를 하나의 문자열로 조합해 Text 컴포넌트로 출력하라. 단 정수부분만 출력되어야 한다.

(예시: 90/100)

조건3 : Slider 컴포넌트를 이용해 HP바를 표현하라. 현재HP와 최대HP의 비율에 맞게 표시되어야 한다.

조건4 : Button 컴포넌트을 이용해 하나는 UP, 다른 하나는 Down으로 버튼 표면 텍스트를 변경한다.

조건5 : 버튼을 누를 때마다 UP 버튼은 현재 HP가 최대 HP의 10%만큼 증가, Down 버튼은 현재 HP가 최대 HP의 10%만큼 감소하는 동작이 실행되어야 한다.

조건6 : 현재 HP는 0보다 크거나 같고 최대 HP보다 작거나 같아야 한다.
*/

public class TempPlayer_230919_고병조 : MonoBehaviour
{
    public float health = 50;    
    public float maxHealth = 100;

    TextMeshProUGUI healthText = null;
    Slider healthSlider = null;
    Button healthUp = null;
    Button healthDown = null;

    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);    
            healthSlider.value = health / maxHealth;
            healthText.text = $"{health:f0}/{maxHealth:f0}";

            //Mathf.FloorToInt  // 올림
            //Mathf.RoundToInt  // 반올림
            //Mathf.Ceil        // 내림
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {        
        Canvas canvas = FindAnyObjectByType<Canvas>();
        Transform child = canvas.transform.GetChild(0);
        healthSlider = child.GetComponent<Slider>();
        child = canvas.transform.GetChild(1);
        healthText = child.GetComponent<TextMeshProUGUI>();
        child = canvas.transform.GetChild(2);
        healthUp = child.GetComponent<Button>();
        child = canvas.transform.GetChild(3);
        healthDown = child.GetComponent<Button>();

        healthUp.onClick.AddListener(UpClick);
        Text upText = healthUp.GetComponentInChildren<Text>();
        upText.text = "UP";

        healthDown.onClick.AddListener(DownClick);
        Text downText = healthDown.GetComponentInChildren<Text>();
        downText.text = "Down";

        Health = health;
    }

    private void DownClick()
    {
        Health -= maxHealth * 0.1f;
    }

    private void UpClick()
    {
        Health += maxHealth * 0.1f;
    }
}
