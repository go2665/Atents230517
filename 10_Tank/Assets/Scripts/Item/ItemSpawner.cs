#define MY_PRE
#define MY_PRE2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;

    public float sizeX = 40.0f;
    public float sizeZ = 40.0f;

    public float baseInterval = 10.0f;
    public float randomRange = 2.0f;

    float current = 0;
    bool isStop = false;

    private void Start()
    {
        current = GetRandomInterval();
    }

    public void StopSpawner()
    {
        isStop = true;
    }

    private void Update()
    {
        current -= Time.deltaTime;
        if(current < 0 && !isStop)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.transform.position = transform.position + new Vector3(Random.Range(0,sizeX), 0, Random.Range(0,sizeZ));

#if UNITY_EDITOR
            Item item = obj.GetComponent<Item>();
            count[(int)item.ShellType]++;
#endif

            current = GetRandomInterval();
        }
    }

    float GetRandomInterval()
    {
        return baseInterval + Random.Range(-randomRange, randomRange);
    }

    private void OnDrawGizmos()
    {
        Vector3 p0 = transform.position;
        Vector3 p1 = transform.position + new Vector3(sizeX, 0, 0);
        Vector3 p2 = transform.position + new Vector3(sizeX, 0, sizeZ);
        Vector3 p3 = transform.position + new Vector3(0, 0, sizeZ);

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }


#if UNITY_EDITOR
    int[] count = new int[3];
    public void Test_Counter()
    {
        Debug.Log($"Total : {count[0]+ count[1]+ count[2]}");
        Debug.Log($"Red : {count[0]}");
        Debug.Log($"Blue : {count[1]}");
        Debug.Log($"Green : {count[2]}");
    }
#endif
}
