using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onLifeTimeChange += Refresh;
    }

    private void Refresh(float ratio)
    {
        slider.value = ratio;
    }

    // 몇초 남았는지 슬라이더 위에 글자로 출력하기
    // 소수점 한자리까지만 출력하기
}
