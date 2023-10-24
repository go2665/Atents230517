using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    TextMeshProUGUI bullet;

    private void Awake()
    {
        bullet = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Inst.Player.onBulletCountChange = OnBulletCountChange;
    }

    private void OnBulletCountChange(int count)
    {
        bullet.text = count.ToString();
    }
}
