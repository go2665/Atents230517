using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Mana Potion", menuName = "Scriptable Object/Item Data - Mana Potion", order = 6)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("마나 포션 데이터")]
    public float mana = 50.0f;

    /// <summary>
    /// 마나 포션을 사용하기 위한 함수
    /// </summary>
    /// <param name="target">이 아이템의 효과를 받을 대상</param>
    /// <returns>true면 사용 성공, false면 사용 못함</returns>
    public bool Use(GameObject target)
    {
        bool result = false;

        if (target != null)                 // 대상이 있어야 하고
        {
            IMana manaTarget = target.GetComponent<IMana>();
            if (manaTarget != null)         // MP가 있어야 한다.
            {
                if(manaTarget.MP < manaTarget.MaxMP)
                {
                    manaTarget.MP += mana;      // MP 증가
                    Debug.Log($"{target.name}의 MP가 {mana}만큼 증가해서 {manaTarget.MP}가 되었습니다.");
                    result = true;
                }
                else
                {
                    Debug.Log($"{target.name}의 MP가 가득 차 있습니다. 사용 실패");
                }
            }
        }

        return result;
    }
}
