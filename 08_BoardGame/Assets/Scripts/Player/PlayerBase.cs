using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// 이 플레이어의 보드
    /// </summary>
    protected Board board;
    public Board Board => board;

    /// <summary>
    /// 이 플레이어가 가지고 있는 함선들
    /// </summary>
    protected Ship[] ships;
    public Ship[] Ships => ships;

    /// <summary>
    /// 아직 침몰하지 않은 함선의 수
    /// </summary>
    protected int remainShipCount;
    public bool IsDepeat => remainShipCount < 1;

    /// <summary>
    /// 이번 턴의 행동이 완료되었는지 여부
    /// </summary>
    bool isActionDone = false;
    public bool IsActionDone => isActionDone;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;


    /// <summary>
    /// 이 플레이어의 공격이 실패했음을 알리는 델리게이트(파라메터:자기자신)
    /// </summary>
    public Action<PlayerBase> onAttackFail;

    /// <summary>
    /// 이 플레이어의 행동이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 이 플레이어가 패배했음을 알리는 델리게이트(파라메타:자기자신)
    /// </summary>
    public Action<PlayerBase> onDefeat;


    // 턴 관리용 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 턴이 시작될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    /// <param name="_">현재 몇번째 턴인지. 사용안함.</param>
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }

    /// <summary>
    /// 턴이 종료될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    public virtual void OnPlayerTurnEnd()
    {
        // 기능없음
    }

    // 공격 관련 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="attackGridPos">공격하는 위치</param>
    public void Attack(Vector2Int attackGridPos)
    {

    }

    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="worldPos">공격하는 위치(월드)</param>
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.WorldToGrid(worldPos));
    }

    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="index">공격하는 위치의 인덱스</param>
    public void Attack(int index)
    {

    }

    /// <summary>
    /// 자동 공격 함수. CPU플레이어나 인간플레이어가 타임아웃되었을 때 사용.
    /// </summary>
    public void AutoAttack()
    {

    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 자동으로 이 플레이어의 보드에 함선을 배치하는 함수
    /// </summary>
    /// <param name="isShowShips"></param>
    public void AutoShipDeployment(bool isShowShips)
    {

    }

    /// <summary>
    /// 함선의 주변 위치들의 인덱스를 구하는 함수
    /// </summary>
    /// <param name="ship">주변 위치를 구할 함선</param>
    /// <returns>함선 주변 위치의 인덱스를 저장한 리스트</returns>
    private List<int> GetShipAtoundPosition(Ship ship)
    {
        return null;
    }

    /// <summary>
    /// 모든 함선의 배치를 취소하는 함수
    /// </summary>
    public void UndoAllShipDeployment()
    {

    }

    // 함선 침몰 및 패배 처리 -----------------------------------------------------------------------
    
    /// <summary>
    /// 내가 가진 특정 배가 파괴되었을 때 실행될 함수
    /// </summary>
    /// <param name="ship">파괴된 배</param>
    private void OnShipDestroy(Ship ship)
    {

    }

    /// <summary>
    /// 모든 배가 침몰했을 때 실행될 함수
    /// </summary>
    private void OnDefeat()
    {

    }

    // 기타 ---------------------------------------------------------------------------------------
    
    /// <summary>
    /// 초기화 함수. 게임 시작 직전 상태로 변경
    /// </summary>
    public void Clear()
    {

    }
}
