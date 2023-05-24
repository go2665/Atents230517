using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 오른쪽으로 계속 날아기기
    public float speed = 7.0f;

    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);    // 기본적으로 local로 처리
        //transform.Translate(Time.deltaTime * speed * Vector2.right, Space.World);// 월드의 오른쪽으로 이동
        
        //transform.position += Time.deltaTime * speed * transform.right;   // 총알의 오른쪽으로 이동
        //transform.position += Time.deltaTime * speed * Vector3.right;     // 월드의 오른쪽으로 이동
    }
}
