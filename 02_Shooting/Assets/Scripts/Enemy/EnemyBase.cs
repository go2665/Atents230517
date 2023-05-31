using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // 왼쪽으로 이동하는 속도
    public float speed = 1.0f;

    // 이 적이 주는 점수
    // [SerializeField]
    public int score = 10;
    public int Score => score;
    
    // 적이 터지는 이팩트
    GameObject explosionEffect;

    private void Awake()
    {
        explosionEffect = GetComponentInChildren<Explosion>(true).gameObject;
    }

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);        
    }

    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * speed * -transform.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))   // 총알만 충돌 처리
        {
            Die();
        }
    }

    // 사망 처리용 함수
    protected virtual void Die()
    {
        explosionEffect.transform.SetParent(null);  // 부모와 같이 죽는 것 방지

        // 특정 회전으로 설정하기
        //explosionEffect.transform.rotation = Quaternion.Euler(0,0,Random.Range(0.0f,360.0f)); 
        // 현재 회전에서 입력받은만큼 추가 회전
        explosionEffect.transform.Rotate(0, 0, UnityEngine.Random.Range(0.0f, 360.0f)); 

        explosionEffect.SetActive(true);    // 활성화 시켜서 보여주기
        Destroy(gameObject);                // 오브젝트 삭제
    }
}
