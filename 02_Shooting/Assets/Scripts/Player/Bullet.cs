using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    // 오른쪽으로 계속 날아기기
    public float speed = 7.0f;

    // 총알의 수명
    public float lifeTime = 10.0f;



    // delegate void OnEnemyKill(int score);   // 델리게이트의 선언(이런 델리게이트가 있다. 리턴타입은 void, 파라메터는 int 하나)
    // OnEnemyKill onEnemyKill;                // OnEnemyKill 타입의 델리게이트 변수 만들기
    
    // 적을 죽였을 때 신호를 보내는 델리게이트
    // public Action<int> onEnemyKill;

    protected override void OnEnable()
    {
        base.OnEnable();

        StopAllCoroutines();                // 혹시 모를 남아있는 코루틴 제거용
        StartCoroutine(LifeOver(lifeTime)); // lifeTime초 후에 비활성화
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);    // 기본적으로 local로 처리
        //transform.Translate(Time.deltaTime * speed * Vector2.right, Space.World);// 월드의 오른쪽으로 이동
        
        //transform.position += Time.deltaTime * speed * transform.right;   // 총알의 오른쪽으로 이동
        //transform.position += Time.deltaTime * speed * Vector3.right;     // 월드의 오른쪽으로 이동
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        Factory.Inst.GetObject(
            PoolObjectType.Hit,
            collision.contacts[0].point,                // 충돌한 지점으로 이팩트 옮기기
            UnityEngine.Random.Range(0.0f, 360.0f));    // 랜덤하게 회전 시키기

        gameObject.SetActive(false);
    }
}
