using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable : 이 클래스는 직렬화 되어야 한다고 표시해 놓은 attribute
[Serializable]
public class SaveData
{
    public string[] rankerNames;    // 랭커들 이름 저장
    public int[] highScores;        // 최고 점수들 저장
}
