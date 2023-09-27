using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    // PlayerBase보다 먼저 실행되어야 한다.
    void Start()
    {
        GameManager.Inst.GameState = GameState.ShipDeployment;        
    }
}
