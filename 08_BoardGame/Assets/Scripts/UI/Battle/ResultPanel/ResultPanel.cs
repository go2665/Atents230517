using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    UserPlayer user;
    EnemyPlayer enemy;

    Button dropDown;    // 아래 보드 부분을 열고 닫는 버튼
    Button restart;     // 게임 재시작 버튼(함선 배치씬으로 이동)

    ResultBoard board;
    ResultAnalysis userAnalysis;
    ResultAnalysis enemyAnalysis;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        dropDown = child.GetComponent<Button>();

        child = transform.GetChild(1);
        board = child.GetComponent<ResultBoard>();

        child = board.transform.GetChild(1);
        userAnalysis = child.GetComponent<ResultAnalysis>();
        child = board.transform.GetChild(2);
        enemyAnalysis = child.GetComponent<ResultAnalysis>();

        child = board.transform.GetChild(3);
        restart = child.GetComponent<Button>();

        dropDown.onClick.AddListener(board.Toggle);
        restart.onClick.AddListener(Restart);
    }

    private void Start()
    {
        user = GameManager.Inst.UserPlayer;
        enemy = GameManager.Inst.EnemyPlayer;

        user.onDefeat += (_) =>
        {
            board.SetDefeat();
            Open();
        };
        enemy.onDefeat += (_) =>
        {
            board.SetVictory();
            Open();
        };

        Close();
    }

    void Open()
    {
        userAnalysis.AllAttackCount = user.SuccessAttackCount + user.FailAttackCount;
        userAnalysis.SuccessAttackCount = user.SuccessAttackCount;
        userAnalysis.FailAttackCount = user.FailAttackCount;
        userAnalysis.SuccessAttackRate = (float)user.SuccessAttackCount / (float)(user.SuccessAttackCount + user.FailAttackCount);

        enemyAnalysis.AllAttackCount = enemy.SuccessAttackCount + enemy.FailAttackCount;
        enemyAnalysis.SuccessAttackCount = enemy.SuccessAttackCount;
        enemyAnalysis.FailAttackCount = enemy.FailAttackCount;
        enemyAnalysis.SuccessAttackRate = (float)enemy.SuccessAttackCount / (float)(enemy.SuccessAttackCount + enemy.FailAttackCount);

        gameObject.SetActive(true);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
