using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_고병조 : MonoBehaviour
{
    public float rotateSpeed = 720.0f;
    public float moveSpeed = 5.0f;

    Transform[] waypoints;
    Transform propeller;
    int targetIndex = 0;

    private void Awake()
    {
        waypoints = new Transform[2];
        waypoints[0] = GameObject.Find("Waypoint1").transform;
        waypoints[1] = GameObject.Find("Waypoint2").transform;        

        propeller = transform.GetChild(4);
    }

    private void Start()
    {
        transform.LookAt(waypoints[0]);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);
        propeller.Rotate(Time.deltaTime * rotateSpeed * transform.forward);

        if((waypoints[targetIndex].position - transform.position).sqrMagnitude < 0.01f )
        {
            GoNextWaypoint();
        }

        transform.LookAt(waypoints[targetIndex]);
    }

    void GoNextWaypoint()
    {
        targetIndex++;

        targetIndex %= waypoints.Length;
        //if( targetIndex >= waypoints.Length )
        //{
        //    targetIndex = 0;
        //}
    }

}
