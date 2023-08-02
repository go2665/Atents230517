using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Regen Potion", menuName = "Scriptable Object/Item Data - Regen Potion", order = 7)]
public class ItemData_RegenPotion : ItemData, IUsable
{
    [Header("재생 포션 데이터")]
    public float heal = 50.0f;
    public float mana = 50.0f;
    public float duration = 2.0f;

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
            IHealth health = target.GetComponent<IHealth>();
            if (health != null)     // HP가 있어야 한다.
            {
                if (health.HP < health.MaxHP)
                {
                    health.HealthRegenetate(heal, duration); // HP 회복
                    Debug.Log($"{target.name}의 HP가 회복시작.");
                    result = true;
                }
                else
                {
                    Debug.Log($"{target.name}의 HP가 가득 차 있습니다. 사용 실패");
                }
            }
            IMana manaTarget = target.GetComponent<IMana>();
            if (manaTarget != null)         // MP가 있어야 한다.
            {
                if(manaTarget.MP < manaTarget.MaxMP)
                {
                    manaTarget.ManaRegenetate(mana, duration);  // MP 회복
                    Debug.Log($"{target.name}의 MP가 회복시작");
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
