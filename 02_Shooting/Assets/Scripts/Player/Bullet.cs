using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 오른쪽으로 계속 날아기기
    public float speed = 7.0f;

    // 총알의 수명
    public float lifeTime = 10.0f;

    GameObject hitExplosion;

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
            hitExplosion.transform.SetParent(null);     // 이팩트의 부모 제거하기
            hitExplosion.transform.position = collision.contacts[0].point;  // 충돌한 지점으로 이팩트 옮기기
            hitExplosion.SetActive(true);               // 이팩트 보여주기

            Destroy(this.gameObject);
        }
    }

    // 총알이 사라지게 만들기
    // 1. 킬존에 닿았을 때
    // 2. 적에게 닿았을 때
    // 3. 생성되고 10초가 지났을 때

    // 총알이 적에게 부딪치면 Hit 이팩트 나오게 만들기
}
