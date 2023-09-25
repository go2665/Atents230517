using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    Button button;
    Image image;
    Transform deployEnd;

    /// <summary>
    /// 선택되었을 때 버튼의 색상
    /// </summary>
    readonly Color selectColor = new(1, 1, 1, 0.2f);

    //bool isToggled = false;
    //bool IsToggled
    //{
    //    get => isToggled;
    //    set
    //    {
    //        if(isToggled != value)
    //        {
    //            isToggled = value;
    //            if(isToggled)
    //            {
    //                image.color = selectColor;
    //                onSelect?.Invoke(this);
    //            }
    //            else
    //            {
    //                image.color = Color.white;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 버튼의 상태를 나타내는 enum
    /// </summary>
    enum DeployState : byte
    {
        NotSelect = 0,  // 선택되지 않은 상태(보통상태)
        Select,         // 선택된 상태(한번 클릭되었을 때 상태)
        Deployed        // 배치 완료된 상태
    }

    /// <summary>
    /// 버튼의 상태
    /// </summary>
    DeployState state;

    /// <summary>
    /// 상태 변경 및 확인용 델리게이트
    /// </summary>
    DeployState State
    {
        get => state;
        set
        {
            if (state != value)     // 상태가 변경될 때만 실행
            {
                state = value;
                switch (state)
                {                    
                    case DeployState.NotSelect:
                        image.color = Color.white;              // 색상 복구
                        deployEnd.gameObject.SetActive(false);  // 배치 완료 텍스트 끄기
                        break;
                    case DeployState.Select:
                        image.color = selectColor;              // 색상을 선택된 색상으로 변경(반투명하게)
                        deployEnd.gameObject.SetActive(false);  // 배치 완료 텍스트 끄기
                        onSelect?.Invoke(this);                 // 선택되었다고 알려서 나머지 버튼 선택 해제
                        break;
                    case DeployState.Deployed:
                        image.color = selectColor;              // 색상 복구
                        deployEnd.gameObject.SetActive(true);   // 배치 완료 텍스트 켜기
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 플레이어(배 배치 때문에 필요)
    /// </summary>
    UserPlayer player;

    /// <summary>
    /// 이 버튼이 배치할 배의 종류
    /// </summary>
    public ShipType shipType = ShipType.None;

    /// <summary>
    /// 이 버튼이 선택되었다고 알리는 델리게이트(파라메터는 자기 자신)
    /// </summary>
    public Action<DeploymentToggle> onSelect;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        deployEnd = transform.GetChild(0);
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;

        Ship ship = player.GetShip(shipType);
        if(ship != null)
        {
            ship.onDeploy += (isDeploy) =>
            {
                if (isDeploy)
                {
                    State = DeployState.Deployed;
                }
                else
                {
                    State = DeployState.NotSelect;
                }
            };
        }
    }

    /// <summary>
    /// 버튼이 클릭되었을 때 실행될 함수
    /// </summary>
    private void OnClick()
    {
        //IsToggled = !IsToggled;
        switch(state) 
        {
            case DeployState.NotSelect:
                // 처음 선택된 것 => 다른 선택된 것들 선택해제. 자신은 선택되었다고 표시
                State = DeployState.Select;
                player.SelectShipToDeploy(shipType);    // 배치를 위해 배 선택                
                break;
            case DeployState.Select:
                // 선택 해제된 것 => 자신에 대해 선택 해제
                State = DeployState.NotSelect;
                break;
            case DeployState.Deployed:
                // 배치 취소 된 것 => 배 배치 취소하고 선택 해제                
                player.UndoShipDeploy(shipType);        // 배 배치 해제
                break;
        }

        // 실습
        // 1. 함선 배치 구현하기
        //   1.1 UserPlayer의 입력 델리게이트 연결 처리
        // 2. 함선 배치 취소하기
        // 3. 버튼 표시 변경
    }

    /// <summary>
    /// 선택 상태로 만드는 함수(패널쪽에서 사용할 것들)
    /// </summary>
    public void SetSelect()
    {

    }

    /// <summary>
    /// NotSelect 상태로 만드는 함수
    /// </summary>
    public void SetNotSelect()
    {
        if(State != DeployState.Deployed)   // 배치된 배를 제외하고 NotSelect로 변경
        {
            State = DeployState.NotSelect;
        }
    }

    public void Test_State(int index)
    {
        State = (DeployState)index;
    }
}
