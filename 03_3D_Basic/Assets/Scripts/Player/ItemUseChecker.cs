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
        IInteractable obj = null;
        do
        {
            obj = target.GetComponent<IInteractable>(); // IInteractable 가져오기 시도
            target = target.parent;                     // target은 부모로 변경
        } while (obj == null && target != null);        // obj를 찾거나 더이상 부모가 없으면 루프 종료

        if( obj != null)
        {
            onItemUse?.Invoke(obj);     // IInteractable를 상속받은 컴포넌트가 있으면 실행
        }
    }
}
