using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    // PlayerBase보다 먼저 실행되어야 한다.
    private void Start()
    {
        GameManager.Inst.GameState = GameState.Battle;

        Ship[] ships = GameManager.Inst.UserPlayer.Ships;
        foreach (Ship ship in ships)
        {
            ship.onHit += (_) => GameManager.Inst.ImpulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized);
            ship.onSinking += (_) => GameManager.Inst.ImpulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized * 3);
        }
        ships = GameManager.Inst.EnemyPlayer.Ships;
        foreach (Ship ship in ships)
        {
            ship.onHit += (_) => GameManager.Inst.ImpulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized);
            ship.onSinking += (_) => GameManager.Inst.ImpulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized * 3);
        }
    }
}
