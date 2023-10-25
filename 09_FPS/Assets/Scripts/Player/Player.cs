using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;

    GunBase activeGun;
    GunBase defaultGun;
    GunBase[] powerGuns;    

    StarterAssets.FirstPersonController controller;

    public Action<int> onBulletCountChange
    {
        get => activeGun.onBulletCountChange;
        set => activeGun.onBulletCountChange = value;
    }


    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        defaultGun = child.GetComponent<GunBase>();
        child = transform.GetChild(4);
        powerGuns = child.GetComponentsInChildren<GunBase>();

        activeGun = defaultGun;        

        controller = GetComponent<StarterAssets.FirstPersonController>();
    }

    private void Start()
    {
        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        
        defaultGun.onFireRecoil += (expend) => crosshair.Expend(expend * 10);

        foreach(var gun in powerGuns)
        {
            gun.onFireRecoil += (expend) => crosshair.Expend(expend * 10);
        }

        activeGun.Equip();
        activeGun.onFireRecoil += GunFireRecoil;
    }

    private void GunFireRecoil(float recoil)
    {
        controller.FireRecoil(recoil);
    }

    /// <summary>
    /// 총 용 카메라 활성화 설정
    /// </summary>
    /// <param name="show">true면 총이 보인다., flase면 총이 안보인다.</param>
    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }

    public void GunFire(bool isFireStart)
    {
        activeGun.Fire(isFireStart);
    }

    public void GunRevolverReload()
    {
        Revolver revolver = activeGun as Revolver;
        if(revolver != null)
        {
            revolver.Reload();
        }
    }

    public void GunChange(GunType type)
    {
        // activeGun 변경

    }
}
