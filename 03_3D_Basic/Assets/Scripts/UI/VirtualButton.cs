using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    Image coolDown;
    public Action onClick;

    void Awake()
    {
        Transform child = transform.GetChild(1);
        coolDown = child.GetComponent<Image>();
        coolDown.fillAmount = 0.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    /// <summary>
    /// 쿨타임 진행상황을 알리는 델리게이트가 실행되면 실행될 함수
    /// </summary>
    /// <param name="ratio">새 비율</param>
    public void RefreshCoolTime(float ratio)
    {
        coolDown.fillAmount = ratio;
    }
}
