using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 표현할 아이템의 종류
    /// </summary>
    ItemData data = null;
    public ItemData ItemData
    {
        get => data;
        set
        {
            if(data == null)    // 팩토리에서 한번 설정가능하도록
            {
                data = value;
            }
        }
    }
}
