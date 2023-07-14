using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public float speed = 1.0f;
    float targetValue = 0;
    float currentValue = 0;

    TextMeshProUGUI killCount;

    private void Awake()
    {
        killCount = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onKillCountChange += OnKillCountChange;
    }

    private void Update()
    {
        currentValue += Time.deltaTime * speed;
        if( currentValue > targetValue )
        {
            currentValue = targetValue;
        }
        killCount.text = Mathf.FloorToInt(currentValue).ToString();
    }

    private void OnKillCountChange(int count)
    {
        targetValue = count;
    }
}
