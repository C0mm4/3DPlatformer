using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemObj
{
    public int upValue;
    public float durate;

    private static Coroutine ItemApplyCoroutine;
    private static float baseSpeed;

    public override void Effect()
    {
        if (ItemApplyCoroutine != null)
        {
            StopCoroutine(ItemApplyCoroutine);

            PlayerManager.Instance.Player.controller.moveSpeed = baseSpeed;
        }

        ItemApplyCoroutine = StartCoroutine(ActiveEffect());
    }

    private IEnumerator ActiveEffect()
    {
        baseSpeed = PlayerManager.Instance.Player.controller.moveSpeed;
        PlayerManager.Instance.Player.controller.moveSpeed += upValue;

        float t = 0;
        while(t <= durate)
        {
            t += Time.deltaTime;

            PlayerManager.Instance.Player.controller.moveSpeed -= upValue / durate * Time.deltaTime;

            yield return null;
        }


        PlayerManager.Instance.Player.controller.moveSpeed = baseSpeed;
    }
}
