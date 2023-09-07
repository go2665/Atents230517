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

    public Action onAttack01Input
    {
        get => button[(int)ButtonType.Bullet].onPress;
        set => button[(int)ButtonType.Bullet].onPress = value;
    }

    public Action onAttack02Input
    {
        get => button[(int)ButtonType.Orb].onPress;
        set => button[(int)ButtonType.Orb].onPress = value;
    }

    public enum ButtonType
    {
        Orb = 0,
        Bullet,
    }

    private void Awake()
    {
        stick = GetComponentInChildren<VirtualStick>();
        button = GetComponentsInChildren<VirtualButton>();
    }
}
