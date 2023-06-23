using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAuto : DoorBase
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            Close();
        }
    }
}
