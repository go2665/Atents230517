using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    void Start()
    {
        GameManager.Inst.GameState = GameState.ShipDeployment;        
    }
}
