using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TurnPrinter : MonoBehaviour
{
    TextMeshProUGUI turnText;

    private void Awake()
    {
        turnText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        turnText.text = $"1 턴";
        TurnManager.Inst.onTurnStart += OnTurnChange;
    }

    private void OnTurnChange(int number)
    {
        turnText.text = $"{number} 턴";
    }
}

// 첫턴이 비정상적으로 동작