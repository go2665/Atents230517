using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeplymentPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널 아래에 붙어있는 모든 배 선택 버튼
    /// </summary>
    DeploymentToggle[] toggles;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeploymentToggle>();  // 자식 한번에 찾아서
        foreach (DeploymentToggle toggle in toggles)
        {
            toggle.onSelect += UnSelectAllOthers;
        }
    }

    /// <summary>
    /// 자신이 아닌 다른 버튼들을 모두 NotSelect 상태로 변경하는 함수
    /// </summary>
    /// <param name="self">지금 선택된 버튼</param>
    private void UnSelectAllOthers(DeploymentToggle self)
    {
        foreach(var toggle in toggles)  // 전부 순회하면서
        {
            if(toggle != self)          // 자신이 아니면
            {
                toggle.SetNotSelect();  // NotSelect 상태로 변경(함수 내부에서 배치된 배는 제외함)
            }
        }
    }
}
