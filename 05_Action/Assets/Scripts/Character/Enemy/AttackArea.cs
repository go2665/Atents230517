using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AttackArea : MonoBehaviour
{
    public Action<IBattle> onPlayerIn;
    public Action<IBattle> onPlayerOut;

    public SphereCollider col;  // 재생중이 아닐 때도 기즈모 그리기 위해서 public 설정

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerIn?.Invoke(battle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            onPlayerOut?.Invoke(battle);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        if(col != null)
        {
            Handles.DrawWireDisc(transform.position, transform.up, col.radius, 5);
        }
    }
#endif
}
