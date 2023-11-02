using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    bool isAlive = true;
    float hp;
    public float MaxHP = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp<=0 && isAlive)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHPChange?.Invoke(hp);
        }
    }
    public Action onDie;
    public Action<float> onHPChange;

    GameObject gunCamera;

    GunBase activeGun;
    GunBase defaultGun;
    GunBase[] powerGuns;    

    StarterAssets.FirstPersonController controller;

    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        defaultGun = child.GetComponent<GunBase>();
        defaultGun.onFireRecoil += GunFireRecoil;

        child = transform.GetChild(4);
        powerGuns = child.GetComponentsInChildren<GunBase>(true);
        foreach (var gun in powerGuns)
        {
            gun.onFireRecoil += GunFireRecoil;
            gun.onBulletCountChange += OnAmmoDepleted;
        }

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

        HP = MaxHP;
        GunChange(GunType.Revoler);
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
        activeGun.gameObject.SetActive(false);

        GunBase newGun = null;
        switch (type)
        {
            case GunType.Revoler:
                newGun = defaultGun;
                break;
            case GunType.Shotgun:
                newGun = powerGuns[0];
                break;
            case GunType.AssaultRifle: 
                newGun = powerGuns[1];
                break;
        }

        activeGun.UnEquip();
        activeGun = newGun;
        activeGun.Equip();
        activeGun.gameObject.SetActive(true);
    }

    public void AddBulletCountChangeDelegate(Action<int> callback)
    {
        defaultGun.onBulletCountChange = callback + defaultGun.onBulletCountChange;
        foreach (var gun in powerGuns)
        {
            gun.onBulletCountChange = callback + gun.onBulletCountChange;
        }
    }

    private void OnAmmoDepleted(int ammo)
    {
        if (ammo <= 0)
        {
            GunChange(GunType.Revoler);
        }
    }

    public Action<float> onAttacked;
    public void Attacked(Enemy enemy)
    {
        Vector3 dir = enemy.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, dir, transform.up);
        onAttacked?.Invoke(-angle);

        HP -= enemy.attackPower;
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("사망");
        onDie?.Invoke();
        gameObject.SetActive(false);
    }
}
