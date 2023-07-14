using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 알파 변하는 속도(1초에 걸쳐서 0->1)
    /// </summary>
    public float alphaChangeSpeed = 1.0f;

    /// 컴포넌트들
    CanvasGroup canvasGroup;
    TextMeshProUGUI playTimeText;
    TextMeshProUGUI killCountText;
    Button restart;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(1);
        playTimeText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        killCountText = child.GetComponent<TextMeshProUGUI>();

        restart = GetComponentInChildren<Button>();
        restart.onClick.AddListener(
            () => StartCoroutine(WaitUnloadAll())
        ); // 버튼이 클릭되었을 때 코루틴 실행하는 람다함수 실행
    }

    private void Start()
    {
        // StopAllCoroutines();         // 필요없어 보이는데 혹시나 싶어서 추가
        Player player = GameManager.Inst.Player;
        player.onDie += OnPlayerDie;    // 플레이어가 죽었을 때 onDie 실행
    }

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 함수
    /// </summary>
    /// <param name="totalPlayTime">전체 플레이 시간</param>
    /// <param name="killCount">죽인 슬라임 수</param>
    private void OnPlayerDie(float totalPlayTime, int killCount)
    {
        playTimeText.text = $"Total Play Time\n\r< {totalPlayTime:f1} Sec >";   // 숫자 세팅
        killCountText.text = $"Total Kill Count\n\r< {killCount} Kill >";
        StartCoroutine(StartAlphaChange()); // 알파 변경 시작
    }

    /// <summary>
    /// 알파값을 1까지 증가 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f) // 알파가 1이 될때까지 반복
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed; // 알파를 지속적으로 증가
            yield return null;          // 다음 프레임까지 대기
        }
        canvasGroup.interactable = true;    // 상호작용 전부 켜기
        canvasGroup.blocksRaycasts = true;  // UI가 레이케스트를 막도록 설정(충돌 시작)
    }

    /// <summary>
    /// 모든 씬이 닫힐 때까지 대기
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitUnloadAll()
    {
        WorldManager world = GameManager.Inst.World;
        while(!world.IsUnloadAll)   // 모든 씬이 다 닫혔는지 확인하면서 대기
        {
            yield return null;
        }
        SceneManager.LoadScene("LoadingScene"); // 다 닫혔으면 LoadingScene열기
    }
}
