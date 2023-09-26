using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipsInfo : MonoBehaviour
{
    public PlayerBase player;
    TextMeshProUGUI[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Ship[] ships = player.Ships;
        for(int i = 0; i < ships.Length; i++)
        {
            PrintHP(texts[i], ships[i]);

            int index = i;
            ships[i].onHit += (ship) => PrintHP(texts[index], ship);
            ships[i].onSinking += (_) => PrintShinking(texts[index]);
        }
    }

    private void PrintHP(TextMeshProUGUI text, Ship ship)
    {
        text.text = $"{ship.HP}/{ship.Size}";
    }

    private void PrintShinking(TextMeshProUGUI text)
    {
        text.fontSize = 40;
        text.text = "<#ff0000>Destroy!!</color>";
    }

}

// 실습
// 1. 함선의 HP를 "현재HP/최대HP" 형식으로 출력하기
// 2. 함선이 침몰하면 빨간색으로 "Destroy!!"로 출력하기
// 3. TurnPrinter 완성하기
