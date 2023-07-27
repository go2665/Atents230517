using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 생성하는 스테틱 클래스
/// </summary>
static public class ItemFactory
{
    /// <summary>
    /// 생성된 아이템의 일련번호
    /// </summary>
    static int itemSerialNumber = 0;

    /// <summary>
    /// 생성 위치에 노이즈를 줄 때 그 강도
    /// </summary>
    static float noisePower = 0.5f;

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <returns></returns>
    public static GameObject MakeItem(ItemCode code)
    {
        ItemData itemData = GameManager.Inst.ItemData[code];                // 코드로 아이템 데이터 가져오기
        GameObject itemObj = GameObject.Instantiate(itemData.modelPrefab);  // 아이템 데이터에 있는 프리팹을 이용해 아이템 오브젝트 생성
        ItemObject item = itemObj.GetComponent<ItemObject>();
        item.ItemData = itemData;                                           // 생성한 아이템 오브젝트에 아이템 데이터 기록

        string[] itemName = itemData.name.Split('_');   // 00_Ruby => 00 Ruby 두개의 스트링으로 나누어 줌 
        itemObj.name = $"{itemName[1]}_{itemSerialNumber++}";               // 이름과 일련번호를 합쳐서 이름 수정

        return itemObj;
    }

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">위치에 노이즈를 추가할지 여부(true면 노이즈 적용)</param>
    /// <returns></returns>
    public static GameObject MakeItem(ItemCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject itemObj = MakeItem(code);
        if(randomNoise)
        {
            // 노이즈를 추가하는 상황이면 랜덤으로 xz 변경
            Vector2 noise = Random.insideUnitCircle * noisePower;
            position.x += noise.x;
            position.z += noise.y;
        }
        itemObj.transform.position = position;  // 지정된 위치로 이동

        return itemObj;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <returns></returns>
    public static GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] itemObjs = new GameObject[count];  // 배열 만들고
        for(int i = 0; i < count;i++)
        {
            itemObjs[i] = MakeItem(code);   // 여러번 생성해서 배열에 담기
        }
        return itemObjs;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">노이즈 적용 여부</param>
    /// <returns></returns>
    public static GameObject[] MakeItems(ItemCode code, uint count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] itemObjs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            itemObjs[i] = MakeItem(code, position, randomNoise);
        }
        return itemObjs;
    }
}
