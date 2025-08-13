using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemObj
{
    public int upValue;
    public float durate;

    // 해당 아이템의 중복 적용 시 baseSpeed 및 Coroutine 인스턴스를 하나로 관리하기 위한 static 선언
    private static Coroutine ItemApplyCoroutine;
    private static float baseSpeed;

    public override void Effect()
    {
        // 아이템 중복 적용을 위한 Coroutine 검사
        if (ItemApplyCoroutine != null)
        {
            StopCoroutine(ItemApplyCoroutine);

            PlayerManager.Instance.Player.controller.moveSpeed = baseSpeed;
        }

        ItemApplyCoroutine = StartCoroutine(ActiveEffect());
    }

    private IEnumerator ActiveEffect()
    {
        // 한번에 속도 증가
        baseSpeed = PlayerManager.Instance.Player.controller.moveSpeed;
        PlayerManager.Instance.Player.controller.moveSpeed += upValue;

        // 점차 감소
        float t = 0;
        while(t <= durate)
        {
            t += Time.deltaTime;

            PlayerManager.Instance.Player.controller.moveSpeed -= upValue / durate * Time.deltaTime;

            yield return null;
        }

        // 종료 시 속도 보정
        PlayerManager.Instance.Player.controller.moveSpeed = baseSpeed;
    }
}
