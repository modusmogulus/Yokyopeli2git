using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SwipeCard : MonoBehaviour, IDragHandler
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        if(_canvas != null) { print("Canvas found");  }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                 _canvas.transform as RectTransform,
                 eventData.position,
                 _canvas.worldCamera,
                 out pos);
        transform.position = _canvas.transform.TransformPoint(pos);
    }
}