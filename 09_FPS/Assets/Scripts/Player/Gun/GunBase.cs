using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunBase : MonoBehaviour
{
    /// <summary>
    /// 총의 사정거리
    /// </summary>
    public float range;

    /// <summary>
    /// 총의 데미지
    /// </summary>
    public float damage;

    /// <summary>
    /// 초당 연사속도
    /// </summary>
    public float fireRate;    

    /// <summary>
    /// 탄 퍼지는 각도
    /// </summary>
    public float spread;

    /// <summary>
    /// 총 반동
    /// </summary>
    public float recoil;

    /// <summary>
    /// 탄창 크기
    /// </summary>
    public int clipSize;

    /// <summary>
    /// 남은 총알 수
    /// </summary>
    public int bulletRemain;

    VisualEffect muzzleEffect;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    public void Fire()
    {
        if( bulletRemain > 0 )
        {
            muzzleEffect.Play();
            bulletRemain--;

            FireProcess();
        }
    }

    protected virtual void FireProcess()
    {
    }

    protected Transform fireTransform;
    public void Equip()
    {
        fireTransform = GameManager.Inst.Player.transform.GetChild(0);
    }
}
