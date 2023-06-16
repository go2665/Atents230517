using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;

    int[] highScores = null;
    string[] rankerNames = null;

    const int rankCount = 5;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScores = new int[rankCount];
        rankerNames = new string[rankCount];
    }

    private void Start()
    {
        SetDefaultData();
    }

    void SetDefaultData()
    {
        for(int i=0;i<rankCount;i++)
        {
            int score = 10;
            for(int j = rankCount-i; j>0 ;j--)
            {
                score *= 10;
            }
            highScores[i] = score;  //100만, 10만, 1만, 1천, 1백

            char temp = 'A';
            temp = (char)((byte)temp + i);
            rankerNames[i] = $"{temp}{temp}{temp}";

            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }
}
