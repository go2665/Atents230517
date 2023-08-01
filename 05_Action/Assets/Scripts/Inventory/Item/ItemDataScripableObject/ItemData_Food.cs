using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Food", menuName = "Scriptable Object/Item Data - Food", order = 3)]
public class ItemData_Food : ItemData, IConsumable
{
    [Header("음식 아이템 데이터")]
    public float heal;
    public float tickTime;
    public uint tickCount;

    public void Consume(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if(health != null )
        {
            health.HealthRegenerateByTick(heal, tickTime, tickCount);
        }
        
    }
}