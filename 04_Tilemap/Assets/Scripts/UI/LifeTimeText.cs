using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeTimeText : MonoBehaviour
{
    TextMeshProUGUI textUI;
    float maxLifeTime;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        maxLifeTime = player.maxLifeTime;
        player.onLifeTimeChange += OnLifeTimeChange;
        textUI.text = $"{maxLifeTime:f2} sec";
    }

    private void OnLifeTimeChange(float ratio)
    {
        textUI.text = $"{(maxLifeTime * ratio):f2} sec";
    }
}