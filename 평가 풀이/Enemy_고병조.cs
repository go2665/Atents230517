using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_고병조 : MonoBehaviour
{
    public float sightRange = 10.0f;
        
    Transform[] waypoints;
    int index = 0;
    Transform target;

    NavMeshAgent agent;
    SphereCollider sphereCollider;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = sightRange;
    }

    private void Start()
    {
        GameObject obj = GameObject.Find("Waypoints");
        Transform way = obj.transform;
        waypoints = new Transform[way.transform.childCount];
        for(int i=0;i<waypoints.Length; i++)
        {
            waypoints[i] = way.GetChild(i);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
    }

    private void Update()
    {
        if( target != null )
        {
            agent.SetDestination(target.position);            
        }
        else if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoNext();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            target = other.transform;
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            target = null;
            ReturnPatrol();
        }
    }

    void GoNext()
    {
        index++;
        index %= waypoints.Length;
        agent.SetDestination(waypoints[index].position);
    }

    void ReturnPatrol()
    {
        agent.SetDestination(waypoints[index].position);
    }
}
