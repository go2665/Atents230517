using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 1. 플레이어의 총에 맞으면 죽는다.

    public float hp = 30.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
        }
    }
    public float maxHp = 30.0f;

    public Action<Enemy> onDie;

    private void OnEnable()
    {
        HP = maxHp;
    }

    private void Die()
    {
        onDie?.Invoke(this);
        gameObject.SetActive(false);
    }
}
