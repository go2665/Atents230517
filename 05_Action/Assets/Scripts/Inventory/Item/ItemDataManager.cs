using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    Ruby = 0,
    Emerald,
    Spaphire
}

// 모든 아이템 종류에 대한 정보를 가지고 있는 관리자 클래스(게임 메니저를 통해 접근 가능)

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// 모든 아이템 종류에 대한 배열
    /// </summary>
    public ItemData[] itemDatas = null;

    /// <summary>
    /// 아이템 종류별 접근을 위한 인덱서
    /// </summary>
    /// <param name="code">접근할 아이템의 코드</param>
    /// <returns>아이템 데이터</returns>
    public ItemData this[ItemCode code] => itemDatas[(int)code];

    /// <summary>
    /// 지금 존재하는 아이템 종류의 모든 갯수
    /// </summary>
    public int length => itemDatas.Length;

}