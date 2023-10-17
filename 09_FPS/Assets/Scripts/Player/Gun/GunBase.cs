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
    /// 현재 발사가 가능한지 여부
    /// </summary>
    private bool isFireReady = true;

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
    int onFireID;

    protected Transform fireTransform;

    public AnimationCurve fireUp;
    public AnimationCurve fireDown;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
        onFireID = Shader.PropertyToID("OnFire");
    }

    public void Fire()
    {
        if(isFireReady && bulletRemain > 0 )
        {
            isFireReady = false;

            muzzleEffect.SendEvent(onFireID);
            bulletRemain--;

            FireProcess();

            StartCoroutine(FireReady());
        }
    }

    IEnumerator FireReady()
    {        
        yield return new WaitForSeconds(1/fireRate);
        isFireReady = true;
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
        Debug.Log($"전체 진행 시간 : {endTime - startTime}");
    }

    protected virtual void FireProcess()
    {
    }

    protected void FireRecoil()
    {
        //Time.timeScale = 0.1f;
        StartCoroutine(FireRecoilCoroutine());
    }

    IEnumerator FireRecoilCoroutine()
    {
        float upTime = 0.05f;
        float elapsedTime = 0.0f;

        Debug.Log(fireTransform.right);

        while(elapsedTime < 1)
        {            
            float angle = -fireUp.Evaluate(elapsedTime) * recoil;
            fireTransform.Rotate(angle, 0, 0);

            elapsedTime += (Time.deltaTime / upTime);

            yield return null;      
        }

        elapsedTime = 0.0f;

        float downTime = 0.2f;
        while (elapsedTime < 1)
        {
            float angle = fireDown.Evaluate(elapsedTime) * recoil * (recoil * 0.05f);  // (recoil * 0.05f)를 곱한 이유는 내려올때 곱하는 회수가 많아 결과값이 증폭되고 있어서 그것을 줄이기 위해 추가          
            fireTransform.Rotate(angle, 0, 0);

            elapsedTime += (Time.deltaTime / downTime); 

            yield return null;
        }
    }

    public void Equip()
    {
        fireTransform = GameManager.Inst.Player.transform.GetChild(0);
    }

    protected Vector3 GetFireDirection()
    {
        Vector3 result = fireTransform.forward;

        result = Quaternion.AngleAxis(Random.Range(-spread, spread), fireTransform.right) * result; // 위 아래로 회전        
        result = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), fireTransform.forward) * result;  // forward 축을 기준으로 0~360사이로 회전

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
