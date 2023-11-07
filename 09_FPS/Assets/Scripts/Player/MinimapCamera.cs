using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public float smooth = 2.0f;
    Vector3 offset;
    Transform target;

    private void Start()
    {
        offset = transform.position;
        target = GameManager.Inst.Player.transform;
        GameManager.Inst.Player.onSpawn += () =>
        {
            transform.position = target.position;
            transform.Rotate(0, target.eulerAngles.y, 0, Space.World);
        };
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
    }
}
