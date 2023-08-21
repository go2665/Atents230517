using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    /// <summary>
    /// 이 탭이 선택되었는지 여부. true면 선택되어 있음, false면 선택되어 있지 않음
    /// </summary>
    bool isSelected = false;

    /// <summary>
    /// 탭의 선택 여부를 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsSelcted
    {
        get => isSelected;
        set
        {
            isSelected = value;
            if(isSelected)
            {
                // 선택 되었다.
                tabImage.color = Color.white;   // 찐하게 보이게 만들기
                onTabSelected?.Invoke(this);    // 선택 되었다고 신호보내기
                SubPanelOpen();               // 서브 패널 열기
            }
            else
            {
                // 선택되지 않았다.
                tabImage.color = UnSelectedColor;   // 반투명하게 보이도록 만들기
                SubPanelClose();                  // 서브패널 닫기
            }
        }
    }

    /// <summary>
    /// 선택되지 않았을 때 보일 반투명한 색상
    /// </summary>
    readonly Color UnSelectedColor = new(1, 1, 1, 0.2f);

    /// <summary>
    /// 탭이 선택되었음을 알리는 델리게이트
    /// </summary>
    public Action<Tab> onTabSelected;

    /// <summary>
    /// 컴포넌트들
    /// </summary>
    Button tabButton;
    Image tabImage;
    TabSubPanel subPanel;

    private void Awake()
    {
        tabButton = GetComponent<Button>();
        tabButton.onClick.AddListener(() => IsSelcted = true);  // 버튼이 클릭되면 선택됨으로 설정됨
        tabImage = GetComponent<Image>();
        subPanel = GetComponentInChildren<TabSubPanel>();

        IsSelcted = false;  // 기본적으로는 선택되지 않음.
    }

    /// <summary>
    /// 서브 패널 여는 함수
    /// </summary>
    public void SubPanelOpen()
    {
        subPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 서브 패널 닫는 함수
    /// </summary>
    public void SubPanelClose()
    {
        subPanel.gameObject.SetActive(false);
    }
}
