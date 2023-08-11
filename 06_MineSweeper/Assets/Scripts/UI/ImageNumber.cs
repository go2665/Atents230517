using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageNumber : MonoBehaviour
{
    // 이 UI로 표시할 숫자. 범위는 (-99~999)
    private int number;

    public int Number
    {
        get => number;
        set
        {
            // 표시되는 숫자 변경하기
        }
    }
}
