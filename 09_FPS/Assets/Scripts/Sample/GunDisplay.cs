using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDisplay : MonoBehaviour
{
    void Start()
    {
        int size = transform.childCount;
        for (int i = 0; i < size; i++)
        {
            Transform child = transform.GetChild(i);
            child.position = i * 2 * Vector3.down;
            child.Rotate(0, 90, 0);
        }
    }
}
