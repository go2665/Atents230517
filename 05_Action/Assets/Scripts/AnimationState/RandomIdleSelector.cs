using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))   // layerIndex 레이어가 트렌지션 중이 아닐 때
        {
            animator.SetInteger("IdleSelect", RandomSelect());
        }
    }

    /// <summary>
    /// 0~4 중에서 랜덤으로 숫자를 골라 리턴하는 함수
    /// </summary>
    /// <returns>0~4</returns>
    int RandomSelect()
    {
        // 각 번호별로 확률이 달라야 한다.
        // 0 이 나올 확률 : 40%
        // 나머지는 각각 : 15%
        int select = 0;

        float num = Random.value;
        if( num < 0.4f )
        {
            select = 0;
        }
        else if( num < 0.55f )
        {
            select = 1;
        }
        else if ( num < 0.7f )
        {
            select = 2;
        }    
        else if( num < 0.85f )
        {
            select = 3;
        }
        else
        {
            select = 4;
        }

        return select;
    }
}
