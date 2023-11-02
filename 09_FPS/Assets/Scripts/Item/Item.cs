using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Transform meshTransform;
    public float spinSpeed = 360.0f;
    

    private void Awake()
    {
        meshTransform = transform.GetChild(0);
    }

    private void Update()
    {
        meshTransform.Rotate(Time.deltaTime * spinSpeed * Vector3.up, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                OnItemConsum(player);
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void OnItemConsum(Player player)
    {
    }
}
