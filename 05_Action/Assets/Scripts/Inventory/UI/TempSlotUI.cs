using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUI_Base
{
    Player owner;

    public Action<bool> onTempSlotOpenClose;

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public override void InitializeSlot(InvenSlot slot)
    {
        onTempSlotOpenClose = null;

        base.InitializeSlot(slot);

        owner = GameManager.Inst.InvenUI.Owner;

        Close();
    }

    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();
        onTempSlotOpenClose?.Invoke(true);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);
        gameObject.SetActive(false);
    }

    public void OnDrop(Vector2 screenPos)
    {
    }
}
