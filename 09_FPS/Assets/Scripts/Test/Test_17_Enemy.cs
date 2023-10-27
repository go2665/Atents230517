using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_17_Enemy : TestBase
{
    //public Enemy enemy;

    public int seed = -1;

    private void Start()
    {
        //enemy.onDie += (target) => Debug.Log($"{target.name} DIE!");
        if( seed != -1)
            Random.InitState(seed);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.transform.position = new(5.5f, 0, -2.5f);
    }
}
