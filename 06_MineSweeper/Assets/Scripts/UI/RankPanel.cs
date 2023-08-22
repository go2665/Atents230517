using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널이 가진 모든 탭
    /// </summary>
    Tab[] tabs;

    /// <summary>
    /// 이 패널에서 선택된 탭
    /// </summary>
    Tab selectedTab;

    /// <summary>
    /// 선택된 탭을 설정하고 확인하는 프로퍼티
    /// </summary>
    Tab SelectedTab
    {
        get => selectedTab;
        set
        {
            if (value != selectedTab)               // 탭이 변경되었을 때
            {
                if( selectedTab != null)
                    selectedTab.IsSelcted = false;  // 선택된 탭이 있으면 선택 해제
                selectedTab = value;                // 새로 선택된 탭 설정
                selectedTab.IsSelcted = true;       // 선택 표시
            }
        }
    }

    /// <summary>
    /// 이 패널에서 사용하는 토글 버튼(탭의 서브패널 열고 닫기용)
    /// </summary>
    ToggleButton toggle;

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();      // 자식 탭 전부 찾기
        foreach (Tab tab in tabs)
        {
            tab.onTabSelected += (newSelectedTab) =>    // 탭이 선택되었을 때
            {
                SelectedTab = newSelectedTab;           // 선택된 탭으로 기록
                toggle.SetToggleState(true);            // 토글도 on으로 설정
            };
        }

        toggle = GetComponentInChildren<ToggleButton>();
        toggle.onToggleChange += (isOn) =>      // 토글 버튼의 상태가 변경되었을 때
        {
            if(isOn)
            {
                SelectedTab.SubPanelOpen();     // on 상태면 서브 패널 열기
            }
            else
            {
                SelectedTab.SubPanelClose();    // off 상태면 서브 패널 닫기
            }
        };
    }

    private void Start()
    {
        GameManager.Inst.onGameClear += Open;       // 게임이 클리어되었을 때 열고
        GameManager.Inst.onGameReady += Close;      // 게임이 초기화 되면 닫기

        Close();    // 시작할 때 처음 닫기
    }

    /// <summary>
    /// 랭크 패널 여는 함수
    /// </summary>
    void Open()
    {
        SelectedTab = tabs[0];          // 켤 때마다 선택된 탭은 첫번째 탭으로 설정
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 랭크 패널 닫는 함수
    /// </summary>
    void Close()
    {
        gameObject.SetActive(false);
    }
}
