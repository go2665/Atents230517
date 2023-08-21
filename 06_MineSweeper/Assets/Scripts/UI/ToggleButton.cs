using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    /// <summary>
    /// 토글버튼이 켜진 상태일 때 보일 이미지
    /// </summary>
    public Sprite onSprite;

    /// <summary>
    /// 토글버튼이 꺼진 상태일 때 보일 이미지
    /// </summary>
    public Sprite offSprite;

    /// <summary>
    /// 토글 버튼이 눌려질 때만다 현재 on/off 상태를 알리는 델리게이트
    /// </summary>
    public Action<bool> onToggleChange;

    /// <summary>
    /// 현재 on/off 상태. true면 on, false면 off
    /// </summary>
    bool isOn = false;

    // 컴포넌트 들
    Image buttonImage;
    Button toggle;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        toggle = GetComponent<Button>();
        toggle.onClick.AddListener(() => SetToggleState(!isOn));    // 버튼이 눌려질 때마다 토글 상태 변경
    }

    private void Start()
    {
        isOn = true;
        SetToggleState(isOn);   // 시작할 때 토글버튼 on으로 설정
    }

    /// <summary>
    /// 토글 상태를 변경하는 함수
    /// </summary>
    /// <param name="on">토글의 새로운 상태. true면 on, false면 off</param>
    public void SetToggleState(bool on)
    {
        // 상태에 따라 스프라이트 변경
        if(on)
        {
            buttonImage.sprite = onSprite;
        }
        else
        {
            buttonImage.sprite = offSprite;
        }
        isOn = on;                      // 상태 기록
        onToggleChange?.Invoke(isOn);   // 상태 알림
    }
}
