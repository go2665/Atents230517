using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCoolTime : MonoBehaviour
{
    public Color emptyColor = Color.red;
    public Color middleColor = Color.yellow;
    public Color fullColor = Color.green;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        PlayerBase tank = GetComponentInParent<PlayerBase>();
        tank.onCoolTimeChange += Refresh;
    }

    private void Refresh(float ratio)
    {
        if(ratio < 0.5f)
        {
            image.color = Color.Lerp(emptyColor, middleColor, ratio * 2);
        }
        else
        {
            image.color = Color.Lerp(middleColor, fullColor, ratio * 2 - 1);
        }
        image.fillAmount = ratio;
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
