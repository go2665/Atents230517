using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    float timeElapsed = 0.0f;

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        Debug.Log(Mathf.Sin(timeElapsed));

        // 적이 물결치듯이 움직이게 만들기
    }
}
