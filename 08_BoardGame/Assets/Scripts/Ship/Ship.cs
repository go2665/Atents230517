using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배의 종류
/// </summary>
public enum ShipType : byte
{
    None = 0,   // 아무배도 아니다(배치 정보에 사용)
    Carrier,    // 항공모함(5칸짜리)
    BattleShip, // 전함(4칸짜리)
    Destroyer,  // 구축함(3칸짜리)
    Submarine,  // 잠수함(3칸짜리)
    PatrolBoat  // 경비정(2칸짜리)
}

/// <summary>
/// 배의 방향(뱃머리가 향하는 방향)
/// </summary>
public enum ShipDirection : byte
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

public class Ship : MonoBehaviour
{
    /// <summary>
    /// 배의 종류
    /// </summary>
    ShipType shipType = ShipType.None;
    public ShipType Type
    {
        get => shipType;
        private set
        {
            shipType = value;
            switch(shipType)
            {
                case ShipType.Carrier:
                    size = 5;
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    break;
                case ShipType.Submarine:
                    size = 3;
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    break;
                default:
                    break;
            }
            // shipName
        }
    } 

    /// <summary>
    /// 배가 바라보는 방향.기본적으로 북->동->남->서->북 순서가 정방향 회전
    /// </summary>
    ShipDirection direction = ShipDirection.North;
    public ShipDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90, 0);
        }
    }

    /// <summary>
    /// 배의 이름
    /// </summary>
    string shipName;

    public string ShipName => shipName;

    /// <summary>
    /// 배의 크기(=최대 HP)
    /// </summary>
    int size = 0;
    public int Size => size;

    /// <summary>
    /// 배의 현재 내구도
    /// </summary>
    int hp = 0;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
        }
    }

    /// <summary>
    /// 배의 생존 여부(true면 살아있다. false면 침몰됐다.)
    /// </summary>
    bool isAlive = true;
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배가 배치된 위치.
    /// </summary>
    Vector2Int[] positions;
    public Vector2Int[] Positions => positions;

    /// <summary>
    /// 배가 배치되었는지 여부(true면 배가 이미 배치되어있다. false면 아직 배치되지 않았다.)
    /// </summary>
    bool isDeployed = false;
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 배의 색상 변경 용
    /// </summary>
    Renderer shipRenderer;

    /// <summary>
    /// 배 모델 메시에 대한 트랜스폼
    /// </summary>
    Transform model;

    /// <summary>
    /// 함선이 배치되거나 배치해제 되었을 때 실행될 델리게이트(파라메터: true면 배치되었다. false면 배치해제되었다.)
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 함선이 공격을 당했을 때 실행될 델리게이트(파라메터:자기자신)
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 함선이 침몰되었을 때 실행될 데리게이트(파라메터:자기자신)
    /// </summary>
    public Action<Ship> onSinking;

    public void Initialize(ShipType type)
    {
        Type = type;

        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();
        
        ResetData();

        gameObject.name = $"{shipType}_{Size}";
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 공통적으로 데이터 초기화하는 함수
    /// </summary>
    void ResetData()
    {

    }

    /// <summary>
    /// 함선의 머티리얼을 선택하는 함수
    /// </summary>
    /// <param name="isNormal">true 불투명 머티리얼 사용, false면 배치모드용 반투명 머티리얼 사용</param>
    public void SetMaterialType(bool isNormal = true)
    {

    }

    /// <summary>
    /// 함선이 배치될 때 실행될 함수
    /// </summary>
    /// <param name="deployPositions">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPositions)
    {
        positions = deployPositions;
        isDeployed = true;
        onDeploy?.Invoke(true);
    }

    /// <summary>
    /// 함선이 배치 해제되었을 때 실행되는 함수
    /// </summary>
    public void UnDeploy()
    {

    }

    /// <summary>
    /// 함선을 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCW">true면 시계방향, false 반시계방향</param>
    public void Rotate(bool isCW = true)
    {
        int dirCount = ShipManager.Inst.ShipDirectionCount;
        if(isCW)
        {
            Direction = (ShipDirection)(((int)Direction + 1) % dirCount);
        }
        else
        {
            Direction = (ShipDirection)(((int)Direction + dirCount - 1) % dirCount);
        }
    }

    /// <summary>
    /// 함선을 랜덤한 방향으로 회전시키는 함수
    /// </summary>
    public void RandomRotate()
    {

    }

    /// <summary>
    /// 함선이 공격을 받았을 때 실행되는 함수
    /// </summary>
    public void OnHitted()
    {

    }    

    /// <summary>
    /// 함선이 침몰할 때 실행되는 함수
    /// </summary>
    void OnSinking()
    {

    }
}
