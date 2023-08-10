using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    /// <summary>
    /// 스킬이 데미지를 입히는 간격
    /// </summary>
    public float skillTick = 0.5f;

    /// <summary>
    /// 스킬이 한틱당 주는 데미지 량(스킬 시전할 때 플레이어의 공격력에 영향을 받음)
    /// </summary>
    public float skillPower = 10.0f;

    /// <summary>
    /// 내 트리거 안에 들어있는 모든 적
    /// </summary>
    List<Enemy> enemies = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemies.Add(enemy);     // 들어온 적 리스트에 추가
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Remove(enemy);  // 나간 적 리스트에서 제거
            }
        }
    }

    private void OnEnable()
    {
        enemies.Clear();                    // 우선 리스트 초기화
        StartCoroutine(SkillCoroutine());   // 코루틴 시작(틱별로 적에게 데미지 주기)
    }

    private void OnDisable()
    {
        StopAllCoroutines();                // 코루틴 끝내기
    }

    /// <summary>
    /// 한틱에 한번씩 적에게 데미지를 주는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator SkillCoroutine()
    {
        while(true) // disable될때까지 계속 반복
        {
            yield return new WaitForSeconds(skillTick); // 틱 시간 대기
            foreach(Enemy enemy in enemies)
            {
                enemy.Defence(skillPower);              // 모든 적을 한번씩 공격
            }
        }
    }
}
