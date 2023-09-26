using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.GameState = GameState.Battle;
    }
}
