using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlanet : MonoBehaviour
{
    // bg-planet를 왼쪽으로 이동 시키기
    // 충분히 왼쪽으로 이동했으면 충분히 오른쪽으로 랜덤하게 보내기
    // 이동할 때 높이도 랜덤으로 바뀌기

    public float moveSpeed = 5.0f;
    public float minRightEnd = 30.0f;
    public float maxRightEnd = 60.0f;
    public float minY = -8;
    public float maxY = -5;
    
    float baseLineX;

    private void Start()
    {
        baseLineX = transform.position.x;        
    }

    private void Update()
    {
        if( transform.position.x < baseLineX )
        {
            // 오른쪽 끝으로 이동하기
            Vector3 newPos = new Vector3(
                Random.Range(minRightEnd, maxRightEnd),
                Random.Range(minY, maxY), 
                0 );
            transform.position = newPos;
        }
        else
        {
            // 왼쪽으로 이동하기
            transform.Translate(Time.deltaTime * moveSpeed * -transform.right);
        }
    }
}
