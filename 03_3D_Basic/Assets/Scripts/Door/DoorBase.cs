using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 종 문들의 기본 클래스
/// </summary>
public class DoorBase : MonoBehaviour
{
    Animator animator;
    readonly int IsOpenHash = Animator.StringToHash("IsOpen");  // 애니메이터에서 사용할 해시

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 문이 열릴때 각 문 종류별로 따로 처리해야 할 일을 오버라이드할 함수
    /// </summary>
    protected virtual void OnOpen()
    {
    }

    /// <summary>
    /// 문이 닫힐때 각 문 종류별로 따로 처리해야 할 일을 오버라이드할 함수
    /// </summary>
    protected virtual void OnClose() 
    {
    }

    /// <summary>
    /// 문을 여는 함수
    /// </summary>
    public void Open()
    {
        animator.SetBool(IsOpenHash, true);
        OnOpen();
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    public void Close()
    {
        OnClose();
        animator.SetBool(IsOpenHash, false);
    }
}
