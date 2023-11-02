using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_17_Enemy : TestBase
{
    //public Enemy enemy;

    public int seed = -1;

    public Revolver revolver;
    public float gunPower = 5;

    private void Start()
    {
        //enemy.onDie += (target) => Debug.Log($"{target.name} DIE!");
        if( seed != -1)
            Random.InitState(seed);

        revolver.fireRate = 10;
        revolver.damage = gunPower;
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        //enemy.transform.position = new(5.5f, 0, -2.5f);

        NavMeshAgent agent = enemy.gameObject.GetComponent<NavMeshAgent>();
        agent.speed = 0.0f;
        agent.Warp(new(5.5f, 0, -2.5f));
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        //PlayerPrefs.SetInt("Level", 100);
        //int level = PlayerPrefs.GetInt("Level");
        //Debug.Log(level);
        //bool result = PlayerPrefs.HasKey("Level");
        //Debug.Log(result);
        //result = PlayerPrefs.HasKey("HP");
        //Debug.Log(result);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.HP -= 10;
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.HP += 10;
    }
}
