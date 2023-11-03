using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Inst.Player.Spawn();
        }
    }
}
