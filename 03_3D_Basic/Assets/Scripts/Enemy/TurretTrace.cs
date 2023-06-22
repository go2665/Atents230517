using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    public float sightRange = 10.0f;
    public float turnSpeed = 2.0f;
    public float fireAngle = 10.0f;

    bool isFiring = false;

    Transform target;
    SphereCollider sightTrigger;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sightTrigger.radius = sightRange;
    }

    private void Update()
    {
        LookTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = null;
        }
    }    

    void LookTarget()
    {
        // target을 바라보는 함수

        if( target != null )
        {
            Vector3 dir = target.position - barrelBodyTransform.position;
            dir.y = 0;
            //barrelBodyTransform.rotation = Quaternion.LookRotation(dir);    // 특정 방향을 바라보는 회전을 만드는 함수
            //barrelBodyTransform.LookAt(target);   // 트랜스폼이 특정 지점을 바라보게 만드는 함수

            barrelBodyTransform.rotation = Quaternion.Slerp(
                barrelBodyTransform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);

            if(!isFiring)
            {
                StartCoroutine(fireCoroutine);
                isFiring = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 2);

        if(barrelBodyTransform == null)
        {
            barrelBodyTransform = transform.GetChild(2);
        }
        
        // 총알 나가는 선 그리기(시야범위까지만)
        Gizmos.DrawLine(barrelBodyTransform.position, 
            barrelBodyTransform.position + barrelBodyTransform.forward * sightRange);
    }
#endif
}
