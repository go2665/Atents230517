using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPad : MonoBehaviour
{
    VirtualStick stick;
    VirtualButton[] button;

    public Action<Vector2> onMoveInput
    {
        get => stick.onMoveInput;
        set => stick.onMoveInput = value;
    }    

    public enum ButtonType
    {
        Bullet,
        Ball
    }

    private void Awake()
    {
        stick = GetComponentInChildren<VirtualStick>();
        button = GetComponentsInChildren<VirtualButton>();
    }
}
