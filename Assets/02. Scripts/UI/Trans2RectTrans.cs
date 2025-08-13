using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trans2RectTrans : MonoBehaviour
{
    public RectTransform targetRect;

    public Canvas canvas; // Canvas ���� �ʿ�

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // ��ũ�� ��ǥ > RectTransform ��ǥ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPos
        );

        targetRect.localPosition = localPos;
    }
}
