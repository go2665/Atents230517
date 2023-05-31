using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    Vector3 destination;

    // 운석은 생성되면 destination 방향이 지정되고 그 방향으로 이동한다.
    // 운석은 항상 반시계방향으로 회전한다.(회전 속도는 랜덤이다.)
}
