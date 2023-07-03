using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePresentation : MonoBehaviour
{
    CinemachineVirtualCamera vCamera;
    CinemachineDollyCart dollyCart;

    private void Awake()
    {
        vCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        dollyCart = GetComponentInChildren<CinemachineDollyCart>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDie += PresentationStart;      // 플레이어가 죽으면 사망 연출용 카메라 움직임 시작

        vCamera.LookAt = player.transform;
    }

    /// <summary>
    /// 사망 연출용 카메라 움직임 처리하는 함수
    /// </summary>
    /// <param name="player">죽은 플레이어</param>
    private void PresentationStart(Player player)
    {
        transform.position = player.transform.position; // 위치를 플레이어 위치로 이동
        vCamera.Priority = 200;     // 우선 순위를 높여서 이 카메라가 촬영하게 말들기
        dollyCart.m_Speed = 5.0f;   // 카트 움직임 시작
    }
}
