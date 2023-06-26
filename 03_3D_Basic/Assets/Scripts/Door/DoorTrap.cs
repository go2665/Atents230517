using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : DoorAuto
{
    /// <summary>
    /// 파티클 시스템 컴포넌트
    /// </summary>
    ParticleSystem ps;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player = null;

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(2);
        
        ps = child.GetComponent<ParticleSystem>();
        ps.gameObject.SetActive(false); // 파티클 시스템 처음에 꺼 놓기
    }

    protected override void OnOpen()
    {
        ps.gameObject.SetActive(true);  // 문이 열리면 파티클 시스템 활성화
        ps.Play();                      // 파티클 시스템 재생 시작
        player.Die();                   // 플레이어 사망
    }

    protected override void OnClose()
    {
        ps.Stop();                      // 파티클 시스템 재생 정지
        ps.gameObject.SetActive(false); // 파티클 시스템 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Player>();  // other에서 플레이어 찾기 시도
        if (player != null)                     // null이 아니면 플레이어라는 소리
        {
            Open();                             // 문을 연다
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();            // 플레이어가 나갔으면 문닫기
            player = null;      // 플레이어 null로 만들기
        }
    }
}
