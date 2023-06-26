using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnTrapActivate(other.gameObject);
    }

    protected virtual void OnTrapActivate(GameObject target) 
    { 
    }
}
