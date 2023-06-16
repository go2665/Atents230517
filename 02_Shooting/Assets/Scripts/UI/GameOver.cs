using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Animator anim;
    Button restart;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        restart = GetComponentInChildren<Button>();
        restart.onClick.AddListener(OnRestart);     // 버튼 클릭되었을 때 OnRestart 함수를 실행되게 만들기
    }

    private void Start()
    {
        // 플레이어가 죽을 때 GameOver 트리거 발동해서 게임 오버창 보이게 만들기
        GameManager.Inst.Player.onDie += (_) => anim.SetTrigger("GameOver");
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(0);  // 씬 다시 불러오기
    }

}
