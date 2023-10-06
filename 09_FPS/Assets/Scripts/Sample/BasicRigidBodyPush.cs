using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	/// <summary>
	/// 밀 대상의 레이어 설정
	/// </summary>
	public LayerMask pushLayers;
	
	/// <summary>
	/// 부딪친 대상을 밀 수 있는지 설정
	/// </summary>
	public bool canPush;

	/// <summary>
	/// 미는 힘
	/// </summary>
	[Range(0.5f, 5f)] public float strength = 1.1f;	
		
	/// <summary>
	/// CharactorController가 이동하다가 충돌이 발생하면 실행되는 이벤트 함수(OnCollisionEnter는 실행안됨)
	/// </summary>
	/// <param name="hit">충돌 정보</param>
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
        // canPush가 true일 때만 동작
        if (canPush) PushRigidBodies(hit);
	}

	/// <summary>
	/// 대상을 미는 함수
	/// </summary>
	/// <param name="hit">충돌 정보</param>
    private void PushRigidBodies(ControllerColliderHit hit)
	{
		// https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

		// 리지드바디가 없거나 키네마틱으로 설정되어 있으면 종료
		Rigidbody body = hit.collider.attachedRigidbody;	// 충돌한 대상의 리지드바디 저장
		if (body == null || body.isKinematic) return;

        // body.gameObject.layer;		// ex) 레이어가 11번째이면 10이 기록되었다.

        // 1		=			0000 0000 0000 0000 0000 0000 0000 0001
        // 1 << 10	=			0000 0000 0000 0000 0000 0100 0000 0000
        // pushLayers.value =	0000 0000 0000 0000 0000 0100 0000 0000

        // 설정한 레이어가 아니면 종료
        int bodyLayerMask = 1 << body.gameObject.layer;		
		if ((bodyLayerMask & pushLayers.value) == 0) return;	// &결과가 0이면 서로 다른 레이어

		// 내가 너무 아래쪽으로 밀고 있으면 종료
		if (hit.moveDirection.y < -0.3f) return;

		// 대상을 밀 방향 계산하기(이동 방향대로 밀기, y는 무시)
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

		// 계산한 방향으로 힘을 가하기
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}