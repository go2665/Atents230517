using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SortPanel : MonoBehaviour
{
    TMP_Dropdown dropdown;
    Button runButton;

    //ItemSortBy selectBy = ItemSortBy.Code;

    public Action<ItemSortBy> onSortRequest;

    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        runButton = GetComponentInChildren<Button>();

        // dropdown.onValueChanged.AddListener((index) => selectBy = (ItemSortBy)index);        
        // runButton.onClick.AddListener(() => onSortRequest?.Invoke(selectBy));

        runButton.onClick.AddListener(() => onSortRequest?.Invoke((ItemSortBy)dropdown.value));
    }
}
