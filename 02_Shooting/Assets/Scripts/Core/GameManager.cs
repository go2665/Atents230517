using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindObjectOfType<Player>();
    }
}
