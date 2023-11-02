using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    TextMeshProUGUI hp;

    private void Awake()
    {
        hp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Inst.Player.onHPChange += OnHPChange;
    }

    private void OnHPChange(float hp)
    {
        this.hp.text = hp.ToString("f0");
    }
}
