using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    public float sightRange = 10.0f;
    public float turnSpeed = 360.0f;
    public float fireAngle = 10.0f;

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
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 2);
    }
#endif
}
