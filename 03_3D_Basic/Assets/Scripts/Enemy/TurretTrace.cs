using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    /// <summary>
    /// 터렛의 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 터렛의 머리가 회전하는 속도
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 발사각
    /// </summary>
    public float fireAngle = 10.0f;

    /// <summary>
    /// 현재 발사 중인지 아닌지 표시. true면 발사중, false면 발사하고 있지 않음
    /// </summary>
    bool isFiring = false;

    /// <summary>
    /// 공격 대상(플레이어)
    /// </summary>
    Transform target;

    /// <summary>
    /// 시야 범위 감지용 트리거
    /// </summary>
    SphereCollider sightTrigger;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sightTrigger.radius = sightRange;   // 시작할 때 sightRange만큼 트리거 크기 조정
    }

    private void Update()
    {
        // 대상이 있는지 확인해서 있으면 해당 방향으로 고개를 돌리다 범위안에 들어오면 공격 시작
        LookTargetAndAttack();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            target = other.transform;   // 플레이어가 트리거 안에 들어오면 타겟으로 지정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = null;  // 플레이어가 트리거에서 나가면 타겟 해제
            StopFire();     // 발사 중이면 발사도 중지
        }
    }    

    /// <summary>
    /// 대상을 바라보고 공격 범위안에 들어있으면 공격하는 함수
    /// </summary>
    void LookTargetAndAttack()
    {
        if( target != null )            // 타겟이 있을 때만
        {   
            Vector3 dir = target.position - barrelBodyTransform.position;   // 방향 계산하고
            dir.y = 0;
            //barrelBodyTransform.rotation = Quaternion.LookRotation(dir);    // 특정 방향을 바라보는 회전을 만드는 함수
            //barrelBodyTransform.LookAt(target);   // 트랜스폼이 특정 지점을 바라보게 만드는 함수

            if (IsVisibleTarget(dir))   // 타겟이 보일 때만
            {
                // 천천히 회전 시키기
                barrelBodyTransform.rotation = Quaternion.Slerp(
                    barrelBodyTransform.rotation, 
                    Quaternion.LookRotation(dir), 
                    Time.deltaTime * turnSpeed);

                // Vector3.Angle : 두 벡터가 이루는 사이각 중 작은 것을 리턴
                // Vector3.SignedAngle : 두 벡터의 사이각을 구하는데 축이되는 벡터를 기준으로 계산

                // 목표지점까지의 각도 계산
                float angle = Vector3.Angle(barrelBodyTransform.forward, dir);
                if (angle < fireAngle)
                {
                    StartFire();    // 발사각 안이면 발사
                }
                else
                {
                    StopFire();     // 밖이면 정지
                }
            }
            else
            {
                StopFire();
            }
        }
    }

    bool IsVisibleTarget(Vector3 lookDir)
    {
        bool result = false;
        if( target != null )
        {
            Ray ray = new(barrelBodyTransform.position, lookDir);

            //int layer = LayerMask.GetMask("Default", "Player", "Wall", "Interactable");
            if ( Physics.Raycast(ray, out RaycastHit hitInfo, sightRange) )
            {
                if ( hitInfo.transform == target )
                {
                    result = true;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 총알을 발사 중이아니면 발사를 시작하는 함수
    /// </summary>
    void StartFire()
    {
        if (!isFiring)
        {
            StartCoroutine(fireCoroutine);
            isFiring = true;
        }
    }

    /// <summary>
    /// 총알이 발사 중이면 발사를 멈추는 함수
    /// </summary>
    void StopFire()
    {
        if (isFiring)
        {
            StopCoroutine(fireCoroutine);
            isFiring = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 2);

        if (barrelBodyTransform == null)
        {
            barrelBodyTransform = transform.GetChild(2);
        }

        Vector3 from = barrelBodyTransform.position;
        Vector3 to;

        // 총알 나가는 선 그리기(시야범위까지만)
        to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;
        Gizmos.color = isFiring ? Color.red : Color.blue;   // 발사중이면 빨간색, 아니면 파란색
        Gizmos.DrawLine(from, to);

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;

        // 발사각 그리기
        Gizmos.color = Color.white;
        to = barrelBodyTransform.position + dir1 * sightRange;
        Gizmos.DrawLine(from, to);
        to = barrelBodyTransform.position + dir2 * sightRange;
        Gizmos.DrawLine(from, to);
    }
#endif
}
