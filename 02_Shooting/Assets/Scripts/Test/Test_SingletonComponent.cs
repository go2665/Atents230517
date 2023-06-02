using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SingletonComponent : Singleton<Test_SingletonComponent>
{
    public int i = 10;
    public int j = 20;
}
