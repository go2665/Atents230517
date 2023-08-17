using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResetButton : MonoBehaviour
{
    /// <summary>
    /// 버튼이 가질 수 있는 상태를 표현하는 enum
    /// </summary>
    enum ButtonState
    {
        Normal = 0,
        Surprise,
        GameClear,
        GameOver
    }

    /// <summary>
    /// 버튼의 현재 상태
    /// </summary>
    ButtonState state = ButtonState.Normal;

    /// <summary>
    /// 버튼의 상태 확인 및 설정용 프로퍼티
    /// </summary>
    ButtonState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                image.sprite = buttonSprites[(int)state];   // 버튼의 상태가 바뀌면 이미지도 변경된다.
            }
        }
    }

    /// <summary>
    /// 버튼에 표시될 스프라이트들
    /// </summary>
    public Sprite[] buttonSprites;


    Image image;
    Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        
    }
}
