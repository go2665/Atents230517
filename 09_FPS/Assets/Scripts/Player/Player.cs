using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;

    GunBase gun;

    public Action<int> onBulletCountChange
    {
        get => gun.onBulletCountChange;
        set => gun.onBulletCountChange = value;
    }


    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;     
        gun = GetComponentInChildren<GunBase>();        
    }

    private void Start()
    {
        gun.Equip();
    }

    /// <summary>
    /// 총 용 카메라 활성화 설정
    /// </summary>
    /// <param name="show">true면 총이 보인다., flase면 총이 안보인다.</param>
    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }

    public void GunFire()
    {
        gun.Fire();
    }

    public void GunRevolverReload()
    {
        Revolver revolver = gun as Revolver;
        if(revolver != null)
        {
            revolver.Reload();
        }
    }
}
