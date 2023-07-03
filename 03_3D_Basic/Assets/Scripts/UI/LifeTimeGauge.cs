using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    TextMeshProUGUI text;
    float maxValue = 1.0f;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onLifeTimeChange += Refresh;
        maxValue = player.lifeTimeMax;
    }

    private void Refresh(float ratio)
    {
        slider.value = ratio;
        text.text = $"{(ratio * maxValue):f1} Sec";
    }
}
