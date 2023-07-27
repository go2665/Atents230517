using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Coin", menuName = "Scriptable Object/Item Data - Coin", order = 2)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null)                 // 동전은 플레이어게만 사용될 수 있다.
        {
            player.Money += (int)price;     // 동전의 가치만큼 플레이어의 돈이 증가
        }
    }
}
