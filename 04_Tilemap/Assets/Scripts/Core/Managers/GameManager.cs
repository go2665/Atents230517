using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player => player;

    /// <summary>
    /// 게임의 전체 맵(월드)을 관리하는 매니저
    /// </summary>
    WorldManager worldManager;
    public WorldManager World => worldManager;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        worldManager = GetComponent<WorldManager>();
        worldManager.PreInitialize();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindObjectOfType<Player>();
        worldManager.Initialize();
    }
}
