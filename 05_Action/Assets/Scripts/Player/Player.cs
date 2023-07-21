using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform weaponParent;
    public Transform shieldParent;

    /// <summary>
    /// 무기와 방패를 표시하거나 표시하지 않는 함수
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowWeaponAndShield(bool isShow)
    {
        weaponParent.gameObject.SetActive(isShow);
        shieldParent.gameObject.SetActive(isShow);
    }
}
