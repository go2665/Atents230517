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

    // 실습
    // 1. 회복 아이템 만들기
    // 2. 플레이어 HP 표시하기
    //  2.1. 숫자로 표시하기
    //  2.2. HP 남은 정도에 따라 화면 어둡게 하기(커브 사용)
    // 3. 적이 죽으면 아이템 떨어트리기
    //  3.1. 확률로 회복아이템, 돌격소총, 샷건
}
