using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무언가를 대신 사용하는 오브젝트
// 이 물체를 사용하면 이 물체에 연결된 다른 오브젝트가 사용된다.
public class DoorSwitch : MonoBehaviour, IInteractable
{
    /// <summary>
    /// 이 스위치가 사용할 오브젝트
    /// </summary>
    public GameObject target;

    /// <summary>
    /// 이 스위치가 사용할 오브젝트의 IInteractable 참조
    /// </summary>
    IInteractable useTarget;

    /// <summary>
    /// 이 스위치가 사용되었는지 여부(true면 사용했음, false면 아직 사용안함)
    /// </summary>
    bool isUsed = false;

    Animator animator;
    
    /// <summary>
    /// 이 물체는 직접 사용한다.
    /// </summary>
    public bool IsDirectUse => true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        bool result = false;
        if( target != null )
        {
            useTarget = target.GetComponent<IInteractable>();   // 대상에서 IInteractable 가져오기
            result = (useTarget != null);
        }

        if( !result )
        {
            // 대상의 IInteractable을 못가져왔으면 워닝
            Debug.LogWarning($"{this.gameObject.name} : 사용할 수 있는 오브젝트가 없습니다.");
        }
    }

    /// <summary>
    /// 이 스위치 사용하는 함수
    /// </summary>
    public void Use()
    {
        if( useTarget != null )     // 이 스위치가 사용할 대상이 있어야 함
        {
            if( !isUsed)            // 이 스위치를 사용하지 않은 상태일 때
            {
                useTarget.Use();                // 대상 오브젝트를 사용
                StartCoroutine(ResetSwitch());  // 일정 시간 뒤에 스위치 리셋
            }
        }
    }

    /// <summary>
    /// 스위치를 리셋하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetSwitch()
    {
        isUsed = true;                          // 사용중이라고 표시
        animator.SetBool("IsOpen", true);       // 사용 애니메이션 재생
        yield return new WaitForSeconds(1);     // 1초 대기
        animator.SetBool("IsOpen", false);      // 스위치 원래 모양으로 초기화
        isUsed = false;                         // 사용중이 아니라고 표시
    }
}
