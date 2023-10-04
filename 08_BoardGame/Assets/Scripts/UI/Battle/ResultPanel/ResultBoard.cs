using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultBoard : MonoBehaviour
{
    public Material victoryMaterial;
    public Material defeatMaterial;
    TextMeshProUGUI victory;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        victory = child.GetComponent<TextMeshProUGUI>();
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetVictory()
    {
        victory.fontMaterial = victoryMaterial;
        victory.text = "승리!";
    }

    public void SetDefeat()
    {
        victory.fontMaterial = defeatMaterial;
        victory.text = "패배...";
    }
}
