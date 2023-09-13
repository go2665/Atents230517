using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    /// <summary>
    /// 함선 프리팹(모델 정보 없음)
    /// </summary>
    public GameObject shipPrefab;

    /// <summary>
    /// 함선의 모델 프리팹(메시만 있음)
    /// </summary>
    public GameObject[] shipModels;

    /// <summary>
    /// 함선의 머티리얼(0: 일반상황용, 1: 배치모드용)
    /// </summary>
    public Material[] shipMaterials;
    public Material NormalShipMaterial => shipMaterials[0];
    public Material DeployModeShipMaterial => shipMaterials[1];

    /// <summary>
    /// 배치모드에서 배치가 가능할 때 사용할 색상
    /// </summary>
    readonly Color successColor = new(0.0f, 1.0f, 0.0f, 0.2f);

    /// <summary>
    /// 배치모드에서 배치가 불가능할 때 사용할 색상
    /// </summary>
    readonly Color failColor = new(1.0f, 0.0f, 0.0f, 0.2f);

    /// <summary>
    /// 배의 종류 수
    /// </summary>
    int shipTypeCount;
    public int ShipTypeCount => shipTypeCount;

    /// <summary>
    /// 배의 방향 갯수
    /// </summary>
    int shipDirectionCount;
    public int ShipDirectionCount => shipDirectionCount;

    /// <summary>
    /// 배의 이름들
    /// </summary>
    readonly public string[] shipNames = { "항공모함", "전함", "구축함", "잠수함", "경비정" };

    protected override void OnInitialize()
    {
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    /// <summary>
    /// 배 게임 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="shipType">생성할 배의 종류</param>
    /// <param name="ownerTransform">생성된 배를 가지는 플레이어의 트랜스폼</param>
    /// <returns>생성완료된 배</returns>
    public Ship MakeShip(ShipType shipType, Transform ownerTransform)
    {
        GameObject shipObj = Instantiate(shipPrefab, ownerTransform);
        GameObject modelPrefab = GetShipModel(shipType);
        Instantiate(modelPrefab, shipObj.transform);

        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(shipType);

        return ship;
    }

    /// <summary>
    /// 함선의 모델 프리팹을 돌려주는 함수
    /// </summary>
    /// <param name="type">필요한 함선의 종류</param>
    /// <returns>함선 프리팹</returns>
    private GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type-1];
    }

    /// <summary>
    /// 배치모드 머티리얼의 색상을 지정하는 함수
    /// </summary>
    /// <param name="isSuccess">true면 successColor로 지정, false면 failColor로 지정</param>
    public void SetDeloyModeColor(bool isSuccess)
    {
        // DeployModeShipMaterial의 색상 변경
    }
}
