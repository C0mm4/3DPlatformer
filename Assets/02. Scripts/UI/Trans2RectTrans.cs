using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trans2RectTrans : MonoBehaviour
{
    public RectTransform targetRect;

    public Canvas canvas; // Canvas 참조 필요

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // 스크린 좌표 > RectTransform 좌표
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPos
        );

        targetRect.localPosition = localPos;
    }
}
