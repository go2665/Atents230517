using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Healing Potion", menuName = "Scriptable Object/Item Data - Healing Potion", order = 5)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    public float heal = 30.0f;

    /// <summary>
    /// 힐링 포션을 사용하기 위한 함수
    /// </summary>
    /// <param name="target">이 아이템의 효과를 받을 대상</param>
    /// <returns>true면 사용 성공, false면 사용 못함</returns>
    public bool Use(GameObject target)
    {
        bool result = false;

        if (target != null)         // 대상이 있어야 하고
        {
            IHealth health = target.GetComponent<IHealth>();
            if (health != null)     // HP가 있어야 한다.
            {
                if( health.HP < health.MaxHP )
                {
                    health.HP += heal;  // HP 증가
                    Debug.Log($"{target.name}의 HP가 {heal}만큼 증가해서 {health.HP}가 되었습니다.");
                    result = true;
                }
                else
                {
                    Debug.Log($"{target.name}의 HP가 가득 차 있습니다. 사용 실패");
                }
            }
        }

        return result;
    }
}
