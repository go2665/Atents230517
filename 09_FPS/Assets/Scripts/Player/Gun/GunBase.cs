using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum GunType
{
    Revoler,
    Shotgun,
    AssaultRifle
}

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
    /// 현재 발사가 가능한지 여부
    /// </summary>
    protected bool isFireReady = true;

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
    int bulletCount;

    protected int BulletCount
    {
        get => bulletCount;
        set
        {
            bulletCount = value;
            onBulletCountChange?.Invoke(bulletCount);
        }
    }

    public Action<int> onBulletCountChange;

    public Action<float> onFireRecoil;

    private VisualEffect muzzleEffect;
    int onFireID;

    protected Transform fireTransform;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
        onFireID = Shader.PropertyToID("OnFire");        
    }

    void Initialize()
    {
        BulletCount = clipSize;
        isFireReady = true;
    }

    public void Fire(bool isFireStart = true)
    {
        if(isFireReady && bulletCount > 0 )
        {                    
            FireProcess(isFireStart);
        }
    }

    IEnumerator FireReady()
    {        
        yield return new WaitForSeconds(1/fireRate);
        isFireReady = true;
    }

    protected void MuzzleEffect()
    {
        muzzleEffect.SendEvent(onFireID);
    }
        
    public IEnumerator TestFire(int count)
    {
        float startTime = Time.unscaledTime;
        
        while (count > 0)
        {
            if(isFireReady)
            {
                Fire();
                count--;
            }
            yield return null;
        }

        float endTime = Time.unscaledTime;
        //Debug.Log($"전체 진행 시간 : {endTime - startTime}");
    }

    protected virtual void FireProcess(bool isFireStart = true)
    {
        isFireReady = false;
        muzzleEffect.SendEvent(onFireID);
        StartCoroutine(FireReady());
        BulletCount--;
    }

    protected void ShotProcess()
    {
        Ray ray = new(fireTransform.position, GetFireDirection());
        if (Physics.Raycast(ray, out RaycastHit hitInfo, range))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                Enemy target = hitInfo.collider.GetComponentInParent<Enemy>();
                HitLocation location = HitLocation.Body;
                if (hitInfo.collider.CompareTag("Enemy_Head"))
                {
                    location = HitLocation.Head;
                }
                else if (hitInfo.collider.CompareTag("Enemy_Arm"))
                {
                    location = HitLocation.Arm;
                }
                else if (hitInfo.collider.CompareTag("Enemy_Leg"))
                {
                    location = HitLocation.Leg;
                }
                target.OnAttacked(location, damage);
            }
            else
            {
                Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
                Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal, reflect);
            }
        }
    }

    protected void FireRecoil()
    {
        //Time.timeScale = 0.1f;
        onFireRecoil?.Invoke(recoil);
    }

    public void Equip()
    {
        fireTransform = GameManager.Inst.Player.transform.GetChild(0);
        Initialize();
    }

    public void UnEquip()
    {
        StopAllCoroutines();
        isFireReady = true;
    }

    protected Vector3 GetFireDirection()
    {
        Vector3 result = fireTransform.forward;

        result = Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), fireTransform.right) * result; // 위 아래로 회전        
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f, 360.0f), fireTransform.forward) * result;  // forward 축을 기준으로 0~360사이로 회전

        //fireDir = result;

        return result;
    }

    //Vector3 fireDir = Vector3.forward;
    void OnDrawGizmos()
    {
        if (fireTransform != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireTransform.forward * range);

            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireDir * range);

        }
    }
}
