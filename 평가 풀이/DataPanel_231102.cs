using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataPanel_231102 : MonoBehaviour
{
    enum SortType
    {
        ByName = 0,
        ByScore,
        ByRatio,
    }

    List<Data> dataList;

    MyTest inputActions;

    TextMeshProUGUI[] names;
    TextMeshProUGUI[] scores;
    TextMeshProUGUI[] ratios;

    private void Awake()
    {
        inputActions = new MyTest();
        Initialize();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += OnTest1;
        inputActions.Test.Test2.performed += OnTest2;
        inputActions.Test.Test3.performed += OnTest3;
    }

    private void OnDisable()
    {
        inputActions.Test.Test3.performed -= OnTest3;
        inputActions.Test.Test2.performed -= OnTest2;
        inputActions.Test.Test1.performed -= OnTest1;
        inputActions.Test.Disable();
    }

    void Initialize()
    {
        Data d1 = new("AAA", 30, 0.5f);
        Data d2 = new("BB", 10, 0.2f);
        Data d3 = new("CCCC", 40, 0.1f);
        Data d4 = new("DDD", 20, 0.4f);
        Data d5 = new("EE", 50, 0.3f);

        dataList = new List<Data>() { d1, d2, d3, d4, d5 } ;

        Transform slots = transform.GetChild(1);
        names = new TextMeshProUGUI[slots.childCount];
        scores = new TextMeshProUGUI[slots.childCount];
        ratios = new TextMeshProUGUI[slots.childCount];

        for(int i=0; i<slots.childCount; i++)
        {
            Transform child = slots.GetChild(i);
            names[i] = child.GetChild(0).GetComponent<TextMeshProUGUI>();
            scores[i] = child.GetChild(1).GetComponent<TextMeshProUGUI>();
            ratios[i] = child.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
    }

    void Sort(SortType type)
    {
        switch (type)
        {
            case SortType.ByName:
                dataList.Sort((x, y) =>
                {
                    if( x.name == null &&  y.name == null )
                    {
                        return 0;
                    }
                    else if( x.name == null)
                    {
                        return -1;
                    }
                    else if( y.name == null )
                    {
                        return 1;
                    }
                    return x.name.CompareTo(y.name);
                });
                break;
            case SortType.ByScore:
                dataList.Sort((x, y) =>
                {
                    return x.score.CompareTo(y.score);
                });
                break;
            case SortType.ByRatio:
                dataList.Sort((x, y) =>
                {
                    return y.ratio.CompareTo(x.ratio);
                });
                break;
        }
    }

    void Refresh()
    {
        int i = 0;
        foreach(var data in dataList)
        {
            names[i].text = data.name;
            scores[i].text = data.score.ToString();
            ratios[i].text = $"{data.ratio:f2}";
            i++;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void OnTest1(InputAction.CallbackContext obj)
    {
        Sort(SortType.ByName);
        Refresh();
    }

    private void OnTest2(InputAction.CallbackContext obj)
    {
        Sort(SortType.ByScore);
        Refresh();
    }

    private void OnTest3(InputAction.CallbackContext obj)
    {
        Sort(SortType.ByRatio);
        Refresh();
    }
}