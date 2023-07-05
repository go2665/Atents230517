using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBlendTree : StateMachineBehaviour
{
    /// <summary>
    /// 미리 찾아 놓을 플레이어
    /// </summary>
    Player player;

    private void OnEnable()
    {
        // 시작할 때 한번만 찾기
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// 트랜지션이 끝날 때나 이 상태가 끝날 때 호출
    /// </summary>
    /// <param name="animator">이 상태를 처리하는 애니메이셔</param>
    /// <param name="stateInfo">애니메이터 관련 정보</param>
    /// <param name="layerIndex">애니메이션 레이어</param>
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RestorInputDir();    // 상태 복구
    }
}
