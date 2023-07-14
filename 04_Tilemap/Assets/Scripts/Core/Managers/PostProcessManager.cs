using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    /// <summary>
    /// 포스트프로세스가 적용되는 볼륨
    /// </summary>
    Volume postProcessVolume;

    /// <summary>
    /// 볼륨안에 있는 비네트 제어용 객체
    /// </summary>
    Vignette vignette;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);   // 프로파일에서 비네트 가져오기(없으면 null)
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;    // 플레이어 수명이 변할 때 실행될 함수 연결
    }

    /// <summary>
    /// 시간변화에 따라 비네트 정도를 조절하는 함수
    /// </summary>
    /// <param name="ratio"></param>
    private void OnLifeTimeChange(float ratio)
    {
        vignette.intensity.value = 1.0f - ratio;
    }
}
