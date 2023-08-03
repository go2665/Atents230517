using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndSkill : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Inst.Player.WeaponEffectEnable(true);
        GameManager.Inst.Player.ShowWeaponAndShield(true);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Inst.Player.WeaponEffectEnable(false);        
    }
}
