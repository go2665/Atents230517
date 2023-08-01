using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    public Color color = Color.white;

    protected Slider slider;
    protected TextMeshProUGUI current;
    protected TextMeshProUGUI max;

    protected float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform child = transform.GetChild(2);
        current = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        max = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(0);
        Image backgroundImage = child.GetComponent<Image>();
        Color backgroundColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
        backgroundImage.color = backgroundColor;

        child = transform.GetChild(1);
        Image fillImage = child.GetComponentInChildren<Image>();
        fillImage.color = color;
    }

    /// <summary>
    /// 바가 표시할 값이 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="ratio">변경된 비율</param>
    protected void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);               // ratio를 0~1로 변경
        slider.value = ratio;                       // 슬라이더 조정
        current.text = $"{(ratio * maxValue):f0}";  // 글자 변경
    }
}
