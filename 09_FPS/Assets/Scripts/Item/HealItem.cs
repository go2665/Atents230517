using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public float heal = 20.0f;

    protected override void OnItemConsum(Player player)
    {
        player.HP += heal;
    }
}
