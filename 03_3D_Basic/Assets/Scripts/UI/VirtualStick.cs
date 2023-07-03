using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    /// <summary>
    /// 전체 영역의 사각형
    /// </summary>
    RectTransform containerRect;

    /// <summary>
    /// 핸들 부분의 사각형
    /// </summary>
    RectTransform handleRect;

    /// <summary>
    /// 핸들이 움직일 수 있는 최대 거리
    /// </summary>
    float stickRange;

    void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;

        // 컨테이너 절반에서 핸들 절반 빼기
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // containerRect 안에서 eventData.position 위치가 containerRect 피봇에서
        // 얼마나 움직였는지를 position에 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(    
            containerRect,                  // 확인할 영역
            eventData.position,             // 확인할 위치
            eventData.pressEventCamera,     // 대상 카메라
            out Vector2 position);          // 결과

        position = Vector2.ClampMagnitude(position, stickRange);    // 최대 영역 안벗어나게 만들기

        handleRect.anchoredPosition = position; // 위치 적용
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Up : {eventData.position}");
        handleRect.localPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"Down : {eventData.position}");        
        // 없으면 Up핸들러가 동작하지 않는다.
    }

    private void InputUpdate(Vector2 position)
    {
        // 델리게이트를 실행
        // 플레이어는 이 델리게이트에 연결이 되어있으면 델리게이트 신호에 맞게 움직인다.
    }
}
