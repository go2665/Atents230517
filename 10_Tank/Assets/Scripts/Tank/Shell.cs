using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float firePower = 10.0f;
    public GameObject explosionPrefab;

    Rigidbody rigid;

    bool isExplosion = false;

    // 1. 생성 되면 즉시 앞으로 날아간다.
    // 2. 포탄이 아닌 다른 것과 부딪치면 폭발한다.
    //  2.1. 주변에 폭팔력을 전달한다.
    //  2.2. 터지는 이팩트가 나온다.(ShellExplosion을 VFX 그래프로 변경해보기)
    // 3. 포탄, 폭팔이팩트는 팩토리로 생성할 수 있다.
    // 4. 포탄, 폭팔이팩트는 오브젝트 풀로 관리된다.

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rigid.velocity = transform.forward * firePower;
        rigid.angularVelocity = Vector3.zero;
        isExplosion = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isExplosion)
        {
            //Time.timeScale = 0.1f;
            isExplosion = true;
            Vector3 pos = collision.contacts[0].point;
            Vector3 normal = collision.contacts[0].normal;
            Instantiate(explosionPrefab, pos, Quaternion.LookRotation(normal));
        }
    }
}
