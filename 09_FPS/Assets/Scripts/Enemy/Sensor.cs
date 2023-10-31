using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Action<GameObject> onSensorTriggered;

    private void OnTriggerEnter(Collider other)
    {
        onSensorTriggered?.Invoke(other.gameObject);
    }
}
