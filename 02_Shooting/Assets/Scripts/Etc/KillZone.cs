using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PooledObject obj = collision.GetComponent<PooledObject>();
        if(obj != null)
        {
            collision.gameObject.SetActive(false);  // 풀 오브젝트는 비활성화
        }
        else
        {
            Destroy(collision.gameObject);  // 킬존 안에 들어온 풀 오브젝트가 아닌 게임 오브젝트는 삭제
        }
    }
}
