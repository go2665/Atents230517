using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public int enemyCount = 10;
    public GameObject enemyPrefab;

    private void Start()
    {
        Enemy testEnemy = null;
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject obj =  Instantiate(enemyPrefab, this.transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.onDie += (target) =>
            {
                StartCoroutine(Respawn(target));
            };

            enemy.transform.position = GetRandomPos();
            testEnemy = enemy;
        }

        testEnemy.transform.position = new(5.5f, 0, -2.5f);
    }

    private Vector3 GetRandomPos()
    {
        int size = CellVisualizer.CellSize;
        return new( Random.Range(0, mazeWidth) * size + 2.5f, 0.0f, -Random.Range(0, mazeHeight) * size - 2.5f);
    }

    IEnumerator Respawn(Enemy target)
    {
        yield return new WaitForSeconds(1);

        target.transform.position = GetRandomPos();
        target.gameObject.SetActive(true);
    }

    // 1. 적을 생성
    // 2. 항상 생성되어 있는 적은 10명 유지
    //  2.1. 시작할 때 10마리 생성
    //  2.2. 플레이어가 적을 죽이면 플레이어로 부터 떨어진 랜덤한 위치에 1초 뒤 생성
}
