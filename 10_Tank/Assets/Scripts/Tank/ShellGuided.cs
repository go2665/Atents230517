using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellGuided : Shell
{
    public float upPower = 20.0f;
    public float guideHeight = 10.0f;
    bool isTracingStart = false;

    private void FixedUpdate()
    {
        if(!isTracingStart && transform.position.y < guideHeight)
        {
            rigid.AddForce(Vector3.up * upPower);
            rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));
        }
        else if(!isTracingStart)
        {
            StartCoroutine(StartTracing());            
        }    
    }

    IEnumerator StartTracing()
    {
        isTracingStart = true;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.useGravity = false;

        float timeElapsed = 0;
        float duration = 1.0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.Rotate(0, 720 * Time.deltaTime, 0, Space.World);
            transform.forward = Vector3.Slerp(transform.forward, Vector3.down, Time.deltaTime * 2);

            yield return null;
        }

        Vector3 findCenter = transform.position;
        findCenter.y = 0;
        float findRadius = guideHeight;

        Collider[] colliders = Physics.OverlapSphere(findCenter, findRadius, LayerMask.GetMask("Players"));
        if(colliders.Length > 0)
        {
            Collider target = colliders[Random.Range(0,colliders.Length)];
            transform.LookAt(target.transform.position);
        }
        rigid.velocity = transform.forward * firePower * 0.5f;
        rigid.useGravity = true;
    }
}
