using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : BarBase
{
    private void Start()
    {
        Player player = GameManager.Inst.Player;
        maxValue = player.MaxHP;
        max.text = $"/ {maxValue}";
        current.text = player.HP.ToString("N0");
        slider.value = player.HP / maxValue;
        player.onHealthChange += OnValueChange;
    }
}
