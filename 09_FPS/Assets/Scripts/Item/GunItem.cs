using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : Item
{
    public GunType gunType;

    protected override void OnItemConsum(Player player)
    {
        player.GunChange(gunType);
    }
}
