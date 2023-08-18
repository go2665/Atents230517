using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    TextMeshProUGUI action;
    TextMeshProUGUI find;
    TextMeshProUGUI notFind;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        action = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        find = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(7);
        notFind = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onActionCountChange += OnActionCountChange;
        gameManager.onGameClear += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameOver += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameReady += () =>
        {
            find.text = "???";
            notFind.text = "???";
        };
    }

    /// <summary>
    /// 행동 횟수가 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="count">새 행동 횟수</param>
    private void OnActionCountChange(int count)
    {
        action.text = count.ToString();
    }

    /// <summary>
    /// 게임이 클리어나 오버되었을 때 실행되는 함수
    /// </summary>
    /// <param name="findCount">찾은 지뢰 개수</param>
    /// <param name="notFindCount">못찾은 지뢰 개수</param>
    private void OnGameEnd(int findCount, int notFindCount)
    {
        find.text = findCount.ToString();
        notFind.text = notFindCount.ToString();
    }
}
