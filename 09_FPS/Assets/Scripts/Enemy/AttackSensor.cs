using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : Sensor
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onSensorTriggered?.Invoke(other.gameObject);
        }
    }
}
