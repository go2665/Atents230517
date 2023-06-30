using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // 플레이어를 천천히 따라가는 느낌의 카메라
    public Transform target;
    public float speed = 3.0f;
    Vector3 offset;
    float length;

    private void Start()
    {
        if(target == null)
        {
            Player player = FindObjectOfType<Player>();
            target = player.transform.GetChild(7);
        }

        offset = transform.position - target.position;  // 플레이어 위치에서 카메라로 가는 벡터
        length = offset.magnitude;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(
            transform.position,
            target.position + Quaternion.LookRotation(target.forward) * offset,
            Time.fixedDeltaTime * speed);   // offset만큼 움직인 위치로 설정
        transform.LookAt(target);           // 바라보는 것은 항상 target을 보도록

        // 타겟(플레이어)에서 카메라로 가는 레이
        Ray ray = new Ray(target.position, transform.position - target.position);
        if( Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            transform.position = hitInfo.point; // 플레이어에서 카메라로 갈 때 가로 막히면 그 지점에 카메라 놓기
        }
    }
}
