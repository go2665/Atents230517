using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipsInfo : MonoBehaviour
{
    public PlayerBase Player;
    TextMeshProUGUI[] texts;
}

// 실습
// 1. 함선의 HP를 "현재HP/최대HP" 형식으로 출력하기
// 2. 함선이 침몰하면 빨간색으로 "Destroy!!"로 출력하기
// 3. TurnPrinter 완성하기
