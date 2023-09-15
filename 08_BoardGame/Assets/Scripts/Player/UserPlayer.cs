using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    /// <summary>
    /// 지금 배치하려는 배
    /// </summary>
    Ship selectedShip;

    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            selectedShip = value;
        }
    }

    // 입력 관련 델리게이트 -------------------------------------------------------------------------
    // 상태별로 따로 처리(만약 null이면 그 상태에서 수행하는 일이 없다는 의미)
    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    // 함선 배치 씬용 입력 함수들 -------------------------------------------------------------------
    private void OnClick_ShipDeployment(Vector2 screen)
    {

    }

    private void OnMouseMove_ShipDeployment(Vector2 screen)
    {

    }

    private void OnMousewheel_ShipDeployment(float wheelDelta)
    {

    }

    // 전투 씬용 입력 함수들 ------------------------------------------------------------------------
    private void OnClick_Battle(Vector2 screen)
    {

    }

    private void OnMouseMove_Battle(Vector2 screen)
    {

    }

    private void OnMousewheel_Battle(float wheelDelta)
    {

    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 특정 종류의 함선을 선택하는 함수
    /// </summary>
    /// <param name="shipType"></param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = ships[(int)shipType - 1];
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 취소하는 함수
    /// </summary>
    /// <param name="shipType">배치를 취소할 함선</param>
    public void UndoShipDeploy(ShipType shipType)
    {

    }
}
