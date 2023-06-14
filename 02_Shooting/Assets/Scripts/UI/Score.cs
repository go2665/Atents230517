using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI scoreUI;

    /// <summary>
    /// 목표로 하는 점수
    /// </summary>
    int targetScore = 0;

    /// <summary>
    /// 현재 점수
    /// </summary>
    float currentScore = 0.0f;

    /// <summary>
    /// 점수가 올라가는 속도
    /// </summary>
    public float scoreUpSpeed = 50.0f;

    private void Awake()
    {
        scoreUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player; // 플레이어 찾고

        currentScore = player.Score;                // 플레이어의 스코어 기준으로 변수 초기화
        targetScore = player.Score;
        scoreUI.text = $"Score : {currentScore:f0}";

        player.onScoreChange += RefreshScore;       // 델리게이트에 함수 연결
    }

    private void Update()
    {
        if (currentScore < targetScore)     // targetScore가 currentScore보다 커지면
        {
            currentScore += Time.deltaTime * scoreUpSpeed;      // 초당 scoreUpSpeed만큼 currentScore 증가
            currentScore = Mathf.Min(currentScore, targetScore);// currentScore가 targetScore보다 커지지 않도록
            scoreUI.text = $"Score : {currentScore:f0}";        // 글자 찍기
        }
    }

    /// <summary>
    /// 플레이어의 점수가 변경될 때마다 실행될 함수
    /// </summary>
    /// <param name="score">변경된 점수</param>
    void RefreshScore(int score)
    {
        targetScore = score;
    }
}
