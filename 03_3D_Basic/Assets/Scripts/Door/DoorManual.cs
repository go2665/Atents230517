using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteractable
{
    public float closeTime = 3.0f;

    TextMeshPro text;

    public bool IsDirectUse => true;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }

    public void Use()
    {
        Open();
        StartCoroutine(AutoClose(closeTime));
    }

    IEnumerator AutoClose(float time)
    {
        yield return new WaitForSeconds(time);
        Close();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player")) 
        { 
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
