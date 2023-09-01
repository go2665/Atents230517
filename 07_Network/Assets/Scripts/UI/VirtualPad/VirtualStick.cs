using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    RectTransform containerRect;
    RectTransform handleRect;
    float stickRange;

    public Action<Vector2> onMoveInput;

    private void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 direction);

        direction = Vector2.ClampMagnitude(direction, stickRange);
        InputUpdate(direction);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // up, down은 세트로 들어가야 하기 때문에 추가
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputUpdate(Vector2.zero);
    }

    void InputUpdate(Vector2 direction)
    {
        handleRect.anchoredPosition = direction;
        onMoveInput?.Invoke(direction / stickRange);
    }
}
