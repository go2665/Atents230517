using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NamePlate : MonoBehaviour
{
    TextMeshPro nameText;

    private void Awake()
    {
        nameText = GetComponentInChildren<TextMeshPro>();
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

}
