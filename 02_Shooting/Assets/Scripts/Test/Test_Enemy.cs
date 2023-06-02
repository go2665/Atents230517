using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Enemy : TestBase
{
    public GameObject prefab;
    public EnemyAsteroid asteroid = null;

    protected override void Test1(InputAction.CallbackContext context)
    {
        if (asteroid == null)
        {
            GameObject obj = Instantiate(prefab);
            asteroid = obj.GetComponent<EnemyAsteroid>();
            asteroid.OnInitialize();
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        if(asteroid != null)
        {
            asteroid.Test_Die();
        }
    }
}
