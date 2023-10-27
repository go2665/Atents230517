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
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject obj =  Instantiate(enemyPrefab, this.transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.onDie += (target) =>
            {
                StartCoroutine(Respawn(target));
            };

            enemy.transform.position = GetRandomPos(true);
        }
    }

    private Vector3 GetRandomPos(bool init = false)
    {
        int size = CellVisualizer.CellSize;

        Vector2Int playerPos = new(-100,-100);
        if (!init)
        {
            Player player = GameManager.Inst.Player;
            playerPos = new Vector2Int(
                (int)(player.transform.position.x / size), -(int)(player.transform.position.z / size));
        }
                
        int x;
        int z;
        
        do
        {
            int index = Random.Range(0, mazeHeight * mazeWidth);
            x = index / mazeWidth;
            z = index % mazeWidth;
        } while (x < playerPos.x + 3 && x > playerPos.x - 3 && z < playerPos.y + 3 && z > playerPos.y - 3);

        Vector3 pos = new Vector3( x * size + 2.5f, 0, -z * size - 2.5f);

        return pos;
    }

    IEnumerator Respawn(Enemy target)
    {
        yield return new WaitForSeconds(1);

        target.transform.position = GetRandomPos();
        target.gameObject.SetActive(true);
    }
}
