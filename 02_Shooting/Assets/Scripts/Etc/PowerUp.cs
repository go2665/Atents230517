using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PooledObject
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 방향 전환되는 시간 간격
    /// </summary>
    public float dirChangeInterval = 1.0f;

    /// <summary>
    /// 방향 전환 최대 갯수
    /// </summary>
    public int dirChangeCountMax = 5;

    /// <summary>
    /// 현재 남아있는 방향 전환 갯수
    /// </summary>
    int dirChangeCount;

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector2 dir;

    IEnumerator DirChange()
    {
        while(true)
        {
            dir = Random.insideUnitCircle;
            dir.Normalize();
            yield return new WaitForSeconds(dirChangeInterval);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        dirChangeCount = dirChangeCountMax;

        StopAllCoroutines();
        StartCoroutine(DirChange());
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Border"))
        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
        }
    }
}
