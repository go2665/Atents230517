using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : PooledObject
{
    // 오른쪽으로 계속 날아기기
    public float speed = 7.0f;

    // 총알의 수명
    public float lifeTime = 10.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        LifeOver(lifeTime); // lifeTime초 후에 gameObject 비활성화하기
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);    // 기본적으로 local로 처리        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   // 적과 부딪치면 내가 사라진다.
        {
            GameObject hitExplosion = Factory.Inst.GetObject(PoolObjectType.Hit);
            hitExplosion.transform.position = collision.contacts[0].point;  // 충돌한 지점으로 이팩트 옮기기
            hitExplosion.transform.Rotate(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));// 랜덤하게 회전 시키기

            gameObject.SetActive(false);
        }
    }

}
