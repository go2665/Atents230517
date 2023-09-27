using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    // 실습
    // 1. 턴 시작시 몇번째 턴이 시작되었는지 출력하기
    //  1.1. 턴 번호는 다른 색으로 출력하기
    // 2. 공격시 공격의 성공/실패 출력하기
    //  2.1. 예) 적의 성공 : "[적의 공격] : [당신]의 [항공모함]에 포탄이 명중했습니다."
    //  2.2. 예) 적의 실패 : "[적의 공격] : [적]의 포탄이 빗나갔습니다."
    //  2.3. 예) 나의 성공 : "[나의 공격] : [적]의 [잠수함]에 포탄이 명중했습니다."
    //  2.4. 예) 나의 실패 : "[나의 공격] : [나]의 포탄이 빗나갔습니다."
    // 3. 함선이 침몰하면 침몰했다고 출력하기
    //  3.1. 예) 내가 침몰 시켰을 경우 : "[나]의 공격 : [적]의 [구축함]이 침몰했습니다."
    //  3.2. 예) 적이 침몰 시켰을 경우 : "[적]의 공격 : [나]의 [전함]이 침몰했습니다."
    // 4. 게임이 종료되면 상황 출력하기
    //  4.1. 예) 내가 이겼을 경우 : "[당신]의 [승리]!";
    //  4.2. 예) 내가 졌을 경우 : "[당신]의 [패배]입니다...";

    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;
    
    const string YOU = "당신";
    const string ENEMY = "적";

    TextMeshProUGUI log;

    const int MaxLineCount = 15;

    List<string> logLines;
    StringBuilder builder;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();
        logLines = new List<string>(MaxLineCount + 5);
        builder = new StringBuilder(MaxLineCount + 5);
    }

    private void Start()
    {
        TurnManager.Inst.onTurnStart += Log_TurnStart;

        UserPlayer user = GameManager.Inst.UserPlayer;
        EnemyPlayer enemy = GameManager.Inst.EnemyPlayer;
        foreach(var ship in user.Ships)
        {
            ship.onHit += (targetShip) => Log_AttackSuccess(false, targetShip);
            ship.onSinking = (targetShip) => { Log_ShipSinking(false, targetShip); } + ship.onSinking;
        }
        foreach(var ship in enemy.Ships)
        {
            ship.onHit += (targetShip) => Log_AttackSuccess(true, targetShip);
            ship.onSinking = (targetShip) => { Log_ShipSinking(true, targetShip); } + ship.onSinking;
        }
        user.onAttackFail += Log_AttackFail;
        enemy.onAttackFail += Log_AttackFail;

        user.onDefeat += Log_Defeat;
        enemy.onDefeat += Log_Defeat;

        Clear();
        Log_TurnStart(1);   // 턴 매니저와의 순서 문제 때문에 강제로 출력
    }

    /// <summary>
    /// 로그를 한 줄 남기는 함수
    /// </summary>
    /// <param name="text">남길 문자열</param>
    public void Log(string text)
    {
        logLines.Add(text);
        if(logLines.Count > MaxLineCount)
        {
            logLines.RemoveAt(0);
        }

        builder.Clear();
        foreach(string line in logLines)
        {
            builder.AppendLine(line);
        }

        log.text = builder.ToString();
    }

    /// <summary>
    /// 로그를 모두 제거하는 함수
    /// </summary>
    public void Clear()
    {
        logLines.Clear();
        log.text = String.Empty;
    }

    /// <summary>
    /// 공격이 성공했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="isUserAttack">true면 유저가 공격, false면 적이 공격했다</param>
    /// <param name="ship">공격을 당한 함선</param>
    void Log_AttackSuccess(bool isUserAttack, Ship ship)
    {
        // "[적의 공격] : [당신]의 [항공모함]에 포탄이 명중했습니다."

        // 공격자 및 피격자용 문자열 설정
        string attackerColor;
        string attackerName;
        string hittedColor;
        string hittedName;
        if(isUserAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;

            hittedColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hittedName = ENEMY; ;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;

            hittedColor = ColorUtility.ToHtmlStringRGB(userColor);
            hittedName = YOU;
        }

        // 함선용 문자열 설정
        string targetShipColor = ColorUtility.ToHtmlStringRGB(shipColor);

        // 최종 문자열 조합
        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{hittedColor}>{hittedName}</color>의 <#{targetShipColor}>{ship.ShipName}</color>에 포탄이 명중했습니다.");
    }

    /// <summary>
    /// 공격이 실패했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="attacker">공격한 플레이어.</param>
    void Log_AttackFail(PlayerBase attacker)
    {
        //적의 실패 : "[적의 공격] : [적]의 포탄이 빗나갔습니다."

        string attackerColor;
        string attackerName;

        if( attacker is UserPlayer )
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }
        Log($"<#{attackerColor}>{attackerName}의 공격</color> : <#{attackerColor}>적</color>의 포탄이 빗나갔습니다.");
    }

    /// <summary>
    /// 함선이 침몰했을 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="isUserAttack">true면 유저가 공격, false면 적이 공격했다</param>
    /// <param name="ship">침몰을 당한 함선</param>
    void Log_ShipSinking(bool isUserAttack, Ship ship)
    {
        // "[나]의 공격 : [적]의 [구축함]이 침몰했습니다."
        string attackerColor;
        string attackerName;
        string sinkedColor;
        string sinkedName;

        if (isUserAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;

            sinkedColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            sinkedName = ENEMY; ;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;

            sinkedColor = ColorUtility.ToHtmlStringRGB(userColor);
            sinkedName = YOU;
        }

        string sinkShipColor;
        sinkShipColor = ColorUtility.ToHtmlStringRGB(shipColor);

        Log($"<#{attackerColor}>{attackerName}</color>의 공격 : <#{sinkedColor}>{sinkedName}</color>의 <#{sinkShipColor}>{ship.ShipName}</color>이 침몰했습니다.");
    }

    /// <summary>
    /// 턴을 시작할 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="number">턴번호</param>
    void Log_TurnStart(int number)
    {
        string colorText = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{colorText}>{number}</color> 번째 턴이 시작되었습니다.");
    }

    /// <summary>
    /// 게임이 끝날 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="player">패배한 플레이어</param>
    void Log_Defeat(PlayerBase player) 
    {
        //  4.1. 예) 내가 이겼을 경우 : "[당신]의 [승리]!";
        //  4.2. 예) 내가 졌을 경우 : "[당신]의 [패배]입니다...";
        if(player is UserPlayer)
        {
            // 내가 졌다.
            Log($"<#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color>의 <#{ColorUtility.ToHtmlStringRGB(enemyColor)}>패배</color>입니다...");
        }
        else
        {
            // 내가 이겼다.
            string colorString = ColorUtility.ToHtmlStringRGB(userColor);
            Log($"<#{colorString}>당신</color>의 <#{colorString}>승리</color>!");
        }
    }
}
