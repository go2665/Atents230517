using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnTarget;
    public float maxY = 4;
    public float minY = -4;
    public float interval = 0.5f;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    protected virtual void Spawn()
    {
        GameObject obj = Instantiate(spawnTarget);
        obj.transform.position = new Vector3(
            transform.position.x,
            Random.Range(minY, maxY),
            0.0f);
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            Spawn();                                    // 적 스폰하기
            yield return new WaitForSeconds(interval);  // interval에 지정된 시간만큼 기다리기
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 0.5f, maxY, 0), 
            new Vector3(transform.position.x + 0.5f, maxY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x + 0.5f, maxY, 0), 
            new Vector3(transform.position.x + 0.5f, minY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x + 0.5f, minY, 0), 
            new Vector3(transform.position.x - 0.5f, minY, 0));
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 0.5f, minY, 0), 
            new Vector3(transform.position.x - 0.5f, maxY, 0));

        //Gizmos.DrawWireCube()
    }

    public void TestSpawn()
    {
        Spawn();
    }
}
