using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSubPanel : MonoBehaviour
{
    /// <summary>
    /// 서브 패널 아래에 있는 모든 RankLine
    /// </summary>
    RankLine[] rankLines;

    /// <summary>
    /// 랭크의 종류
    /// </summary>
    public enum RankType
    {
        Action = 0, // 행동 기준
        Time        // 시간 기준
    }

    /// <summary>
    /// 이 서브패널이 표현할 랭크의 종류
    /// </summary>
    public RankType rankType = RankType.Action;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
    }

    private void Start()
    {
        string countWord;
        switch (rankType)   // 랭크 종류에 따라 countWord 결정
        {
            case RankType.Action:
                countWord = "회";
                break;
            case RankType.Time:
                countWord = "초";
                break;
            default:
                countWord = "";
                break;
        }

        foreach (RankLine rankLine in rankLines)
        {
            rankLine.SetCountWord(countWord);
        }

        GameManager.Inst.onGameClear += Refresh;    // 게임 클리어 되었을 때 Refresh 실행
        Refresh();
    }

    /// <summary>
    /// 현재 랭킹 정보를 UI에 갱신
    /// </summary>
    public void Refresh()
    {
        RankDataManager rankData = GameManager.Inst.RankDataManager;

        int index = 0;
        switch (rankType)   // 랭크 종류별로 리셋
        {
            case RankType.Action:
                foreach(RankData<int> data in rankData.ActionRank)
                {
                    rankLines[index].SetData(index + 1, data.Data, data.Name);
                    index++;
                }
                break;
            case RankType.Time:
                foreach (RankData<float> data in rankData.TimeRank)
                {
                    rankLines[index].SetData(index + 1, data.Data, data.Name);
                    index++;
                }
                break;
            default:
                break;
        }
        for(int i=index; i<rankLines.Length; i++)   // 남아있는 rankLine은 전부 안보이게 설정
        {
            rankLines[i].ClearLine();
        }
    }
}
