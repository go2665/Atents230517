using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseChecker : MonoBehaviour
{
    public Action<IInteractable> onItemUse;

    private void OnTriggerEnter(Collider other)
    {
        // 체커의 트리거 영역에 다른 컬라이더가 들어왔을 때
        Transform target = other.transform;
        while (target.parent != null)
        {
            target = target.parent;     // 최상위 부모까지 올라간 후
        }

        IInteractable obj = target.GetComponent<IInteractable>();
        if( obj != null)
        {
            onItemUse?.Invoke(obj);     // IInteractable를 상속받은 컴포넌트가 있으면 실행
        }
    }
}
