using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BloodOverlay : MonoBehaviour
{
    public AnimationCurve curve;

    //float min = 0;      // hp가 50%일 때 알파값
    //float max = 0.8f;   // hp가 0%일 때 알파값

    float inverseMaxHP;

    Image image;
    public Color color = Color.clear;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = color;
    }

    private void Start()
    {
        inverseMaxHP = 1 / GameManager.Inst.Player.MaxHP;
        GameManager.Inst.Player.onHPChange += OnHPChange;
    }

    private void OnHPChange(float hp)
    {
        float ratio = 1 - hp * inverseMaxHP;

        color.a = curve.Evaluate(ratio);
        image.color = color;
    }
}
