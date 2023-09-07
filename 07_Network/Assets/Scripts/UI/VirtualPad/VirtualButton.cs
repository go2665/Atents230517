using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler
{
    public Action onPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPress?.Invoke();
    }
}
