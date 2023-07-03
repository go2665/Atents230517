using System;
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

    /// <summary>
    /// 가상 스틱의 입력을 알리는 델리게이트
    /// </summary>
    public Action<Vector2> onMoveInput;

    void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;

        // 컨테이너 절반에서 핸들 절반 빼기
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    /// <summary>
    /// 드래그가 될 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
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

        InputUpdate(position);        
    }

    /// <summary>
    /// 마우스가 떨어졌을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Up : {eventData.position}");
        
        InputUpdate(Vector2.zero);  // 스틱 중립으로 바꾸고 멈추라는 신호 보내기
    }

    /// <summary>
    /// 마우스가 눌러졌을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"Down : {eventData.position}");        
        // 없으면 Up핸들러가 동작하지 않는다.
    }

    /// <summary>
    /// 입력에 따라 핸들 움직이고 신호 보내는 함수
    /// </summary>
    /// <param name="position">움직인 정도(스크린좌표로)</param>
    private void InputUpdate(Vector2 position)
    {        
        handleRect.anchoredPosition = position;     // 위치 적용
        onMoveInput?.Invoke(position/stickRange);   // 입력 신호 보내기( (-1,-1) ~ (1,1)로 변환해서 보냄 )
    }
}
