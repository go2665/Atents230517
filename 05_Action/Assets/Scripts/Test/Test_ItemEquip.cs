using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemEquip : TestBase
{
    private void Start()
    {
        ItemFactory.MakeItem(ItemCode.IronSword);
        ItemFactory.MakeItem(ItemCode.SilverSword);
        ItemFactory.MakeItem(ItemCode.OldSword);
        ItemFactory.MakeItem(ItemCode.RoundShield);
        ItemFactory.MakeItem(ItemCode.KiteShield);
    }
}

// 코드 확인
// 장비를 한 아이템을 드래그해서 위치를 변경했을 때 E 마크가 사라지는 문제 수정해보기
// 무기 이팩트 처리