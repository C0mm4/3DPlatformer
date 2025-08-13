using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [Header("Layers")]
    public Image backLayer;    // 회복 시 미리 표시 (녹색)
    public Image middleLayer;  // 체력 감소 시 지연 애니메이션
    public Image frontLayer;   // 현재 체력 즉시 표시

    [Header("Settings")]
    public float decreaseAnimTime = 0.5f; // 감소 애니메이션 속도
    public float increaseAnimTime = 0.5f; // 회복 애니메이션 속도

    private Coroutine coroutine;

    private float targetFill;


    Condition targetCondition;
    enum ConditionType
    {
        HP, Stamina
    }
    [SerializeField]
    private ConditionType type;


    public void OnChangeValue(float increaseValue)
    {
        // 목표 Condition 검출
        targetCondition = FindTargetCondition();

        // 목표 값 정규화
        float normalizeValue = targetCondition.GetPercentage();
        targetFill = Mathf.Clamp01(normalizeValue);

        if (coroutine != null)
            StopCoroutine(coroutine);

        if (increaseValue <= 0)
        {
            // 감소
            frontLayer.fillAmount = targetFill; // 앞 레이어 즉시 감소
            coroutine = StartCoroutine(DecreaseAnimation());
        }
        else
        {
            // 증가
            backLayer.fillAmount = targetFill; // 뒷 에리어 즉시 증가
            
            coroutine = StartCoroutine(IncreaseAnimation());
        }
    }

    public Condition FindTargetCondition()
    {
        switch (type)
        {
            case ConditionType.HP:
                return targetCondition = PlayerManager.Instance.Player.condition.HP;
            case ConditionType.Stamina:
                return targetCondition = PlayerManager.Instance.Player.condition.Stamina;
        }
        return new Condition();
    }

    public void OnChangePercentage(float percentage)
    {
        targetFill = Mathf.Clamp01(percentage);

        if (coroutine != null)
            StopCoroutine(coroutine);

        // 모든 레이어를 동일하게 맞춤
        frontLayer.fillAmount = targetFill;
        middleLayer.fillAmount = targetFill;
        backLayer.fillAmount = targetFill;
    }

    private IEnumerator DecreaseAnimation()
    {
        float startMiddle = middleLayer.fillAmount;
        float startBack = backLayer.fillAmount;
        float elapsed = 0f;

        while (elapsed < decreaseAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / decreaseAnimTime;
            backLayer.fillAmount = Mathf.Lerp(startBack, targetFill, t); // 부드럽게 감소
            middleLayer.fillAmount = Mathf.Lerp(startMiddle, targetFill, t);
            yield return null;
        }
        OnChangePercentage(targetFill);
    }

    private IEnumerator IncreaseAnimation()
    {
        float start = frontLayer.fillAmount;
        float elapsed = 0f;
        while (elapsed < increaseAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / increaseAnimTime;
            frontLayer.fillAmount = Mathf.Lerp(start, targetFill, t);
            yield return null;
        }
        OnChangePercentage(targetFill);
    }
}
