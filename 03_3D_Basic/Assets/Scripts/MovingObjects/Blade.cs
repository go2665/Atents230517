using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WaypointUser
{
    public float spinSpeed = 720.0f;
    Transform bladeMesh;
        
    protected override Transform Target 
    { 
        get => base.Target;
        set
        {
            base.Target = value;
            transform.LookAt(Target);   // 이동 방향을 바라보아야 한다.
        }
    }

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
    }

    void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * spinSpeed * Vector3.right); // 날부분은 계속 회전
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if( player != null)
        {
            player.Die();   //플레이어가 닿으면 플레이어가 죽어야 한다.
        }
    }
}