using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    ShellType shellType = ShellType.Normal;

    private void Start()
    {
        Array enums = Enum.GetValues(typeof(ShellType));

        shellType = (ShellType)(UnityEngine.Random.Range(0, enums.Length));

        Renderer renderer = GetComponentInChildren<Renderer>();
        switch(shellType)
        {
            case ShellType.Normal:
                renderer.material.color = Color.red;
                break;
            case ShellType.Guided:
                renderer.material.color = Color.green;
                break;
            case ShellType.Clust:
                renderer.material.color = Color.blue;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int targetLayer = collision.gameObject.layer;
        int playersLayer = LayerMask.NameToLayer("Players");

        if (targetLayer == playersLayer)
        {
            PlayerBase playerBase = collision.gameObject.GetComponent<PlayerBase>();
            playerBase.SetShell(shellType);

            Destroy(this.gameObject);
        }
    }
}
