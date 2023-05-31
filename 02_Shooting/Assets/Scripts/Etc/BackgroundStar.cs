using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStar : Background
{
    // bg-stars를 이용해서 배경 위에 별 표시하기
    // bg-stars는 오른쪽으로 갈때마다 랜덤으로 xy가 플립된다.

    SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        base.Awake();

        // GetComponentsInChildren
        // 나와 내 자식에 있는 SpriteRenderer를 모두 찾아 배열에 담은 후 리턴하는 함수
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();    
    }

    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);

        // C#에서 숫자 앞에 0b_를 붙이면 이진수라는 의미 (0b_0100_1010), 10진수 74
        // C#에서 숫자 앞에 0x를 붙이면 16진수라는 의미 (0x4a), 10진수 74
        int rand = Random.Range(0, 4);  // 0(0b_00),1(0b_01),2(0b_10),3(0b_11) 중 하나로 랜덤 선택

        // rand의 첫번째 비트가 0이냐 1이냐, true면 첫번째 비트는 1, false면 첫번째 비트는 0
        spriteRenderers[index].flipX = ((rand & 0b_01) != 0);
        // rand의 두번째 비트가 0이냐 1이냐, true면 두번째 비트는 1, false면 두번째 비트는 0
        spriteRenderers[index].flipY = ((rand & 0b_10) != 0);
    }
}
