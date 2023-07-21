using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 한 종류에 대한 기본 정보를 가지는 스크립터블 오브젝트
// 스크립터블 오브젝트 : 데이터 파일의 양식을 설정할 수 있게 해주는 클래스

[CreateAssetMenu(fileName ="New Item Data",menuName ="Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 데이터")]
    public ItemCode code;                       // 아이템 코드
    public string itemName = "아이템";           // 아이템 이름
    public GameObject modelPrefab;              // 아이템이 씬에 있을 때의 모델용 프리팹
    public Sprite itemIcon;                     // 아이템이 인벤토리 안에서 보일 아이콘
    public uint price = 0;                      // 아이템 가치
    public uint maxStackCount = 1;              // 아이템이 인벤토리 슬롯에서 최대 몇개싸지 누적될 수 있는지
    public string itemDescription = "설명";      // 아이템의 상세 설명
}
