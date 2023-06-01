using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 오른쪽으로 계속 날아기기
    public float speed = 7.0f;

    // 총알의 수명
    public float lifeTime = 10.0f;

    // 터지는 이팩트
    GameObject hitExplosion;


    // delegate void OnEnemyKill(int score);   // 델리게이트의 선언(이런 델리게이트가 있다. 리턴타입은 void, 파라메터는 int 하나)
    // OnEnemyKill onEnemyKill;                // OnEnemyKill 타입의 델리게이트 변수 만들기
    
    // 적을 죽였을 때 신호를 보내는 델리게이트
    // public Action<int> onEnemyKill;

    private void Awake()
    {
        hitExplosion = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);  // lifeTime초 후에 gameObject 삭제하기
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
        if (collision.gameObject.CompareTag("Enemy"))   // 적과 부딪치면 내가 사라진다.
        {
            Debug.Log($"{collision.gameObject.name}");
            hitExplosion.transform.SetParent(null);     // 이팩트의 부모 제거하기
            hitExplosion.transform.position = collision.contacts[0].point;  // 충돌한 지점으로 이팩트 옮기기
            hitExplosion.transform.Rotate(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));// 랜덤하게 회전 시키기
            hitExplosion.SetActive(true);               // 이팩트 보여주기

            // EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();   // 태그가 Enemy니까 EnemyBase는 무조건 있음
            // onEnemyKill?.Invoke(enemy.Score);   // onEnemyKill에 연결된 함수를 모두 실행하기(하나도 없으면 실행안함)

            Destroy(this.gameObject);
        }
    }

}
