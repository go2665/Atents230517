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
    /// 아이템 데이터 메니저
    /// </summary>
    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    /// <summary>
    /// 인벤토리 UI
    /// </summary>
    InventoryUI inventoryUI;
    public InventoryUI InvenUI => inventoryUI;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindObjectOfType<Player>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
}
