using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    public float skillTick = 0.5f;
    public float skillPower = 10.0f;

    // 내 트리거 안에 들어있는 모든 적
    List<Enemy> enemies = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemies.Add(enemy);
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
                enemies.Remove(enemy);
            }
        }
    }

    private void OnEnable()
    {
        enemies.Clear();
        StartCoroutine(SkillCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator SkillCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(skillTick);
            foreach(Enemy enemy in enemies)
            {
                enemy.Defence(skillPower);
            }
        }
    }
}
