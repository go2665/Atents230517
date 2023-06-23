using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 오브젝트와 닿으면 연결된 문이 열린다.
// 플레이어와 이 오브젝트가 닿으면 이 오브젝트는 사라진다.
// 이 오브젝트는 제자리에서 빙글빙글 돈다.

public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    public float rotateSpeed = 360.0f;

    Transform keyTransform;

    private void Awake()
    {
        keyTransform = transform.GetChild(0);
    }

    private void Update()
    {
        keyTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);        
    }

    private void OnTriggerEnter(Collider other)
    {
        target.Open();
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        if(target != null)
        {
            // as : 왼쪽에 있는 변수가 오른쪽에 있는 타입이면 null이 아니고 오른쪽에 있는 타입이 아니면 null이다.
            // is : 왼쪽에 있는 변수가 오른쪽에 있는 타입이면 true고 오른쪽에 있는 타입이 아니면 false이다.
            target = target as DoorAuto;    
        }
    }
}
