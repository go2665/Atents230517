using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 랭킹 하나에 대한 정보를 저장하는 클래스
/// </summary>
/// <typeparam name="T">랭킹 정보용 데이터 타입</typeparam>
public class RankData<T>
{
    readonly T data;
    public T Data => data;
    readonly string name;
    public string Name => name;

    // 무조건 new할 때 데이터 세팅하기
    public RankData(T data, string name)
    {
        this.data = data;
        this.name = name;
    }
}

/// <summary>
/// 랭킹정보를 관리하는 클래스
/// </summary>
public class RankDataManager : MonoBehaviour
{
    public int rankCount = 10;

    List<RankData<int>> actionRank;
    public List<RankData<int>> ActionRank => actionRank;

    List<RankData<float>> timeRank;
    public List<RankData<float>> TimeRank => timeRank;

    private void Awake()
    {
        actionRank = new List<RankData<int>>(rankCount + 1);
        timeRank = new List<RankData<float>>(rankCount + 1);
    }

    // 저장
    void SaveData()
    {

    }

    // 불러오기
    void LoadData()
    {

    }

    // 갱신
    void UpdateData(int actionCount, float playTime, string rankerName)
    {
        actionRank.Add(new(actionCount, rankerName));
        timeRank.Add(new(playTime, rankerName));

        // actionRank와 timeRank를 정렬하고 rankCount개수만 남겨 놓기
        // IComparable 사용 권장

    }

    // 테스트용
    public void Test_ActionRankSetting()
    {
        actionRank = new List<RankData<int>>(10);

        actionRank.Clear();

        actionRank.Add(new(1, "AAA"));
        actionRank.Add(new(5, "BBB"));
        actionRank.Add(new(10, "CCC"));
        actionRank.Add(new(15, "DDD"));
        actionRank.Add(new(20, "EEE"));
    }

    public void Test_TimeRankSetting()
    {
        timeRank = new List<RankData<float>>(10);

        timeRank.Clear();

        timeRank.Add(new(1.55f, "AAA"));
        timeRank.Add(new(5.55f, "BBB"));
        timeRank.Add(new(10.55f, "CCC"));
        timeRank.Add(new(15.55f, "DDD"));
        timeRank.Add(new(20.55f, "EEE"));
    }

    public void Test_Update(int actionCount, float playTime, string rankerName)
    {
        rankCount = 5;
        UpdateData(actionCount, playTime, rankerName);
    }
}
