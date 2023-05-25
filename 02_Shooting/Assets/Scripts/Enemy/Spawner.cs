using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnTarget;
    public float maxY = 4;
    public float minY = -4;

    private void Spawn()
    {
        GameObject obj = Instantiate(spawnTarget);
        obj.transform.position = new Vector3(
            transform.position.x,
            Random.Range(minY, maxY),
            0.0f);
    }

    public void TestSpawn()
    {
        Spawn();
    }
}
