using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [Header("Layers")]
    public Image backLayer;    // ȸ�� �� �̸� ǥ�� (���)
    public Image middleLayer;  // ü�� ���� �� ���� �ִϸ��̼�
    public Image frontLayer;   // ���� ü�� ��� ǥ��

    [Header("Settings")]
    public float decreaseAnimTime = 0.5f; // ���� �ִϸ��̼� �ӵ�
    public float increaseAnimTime = 0.5f; // ȸ�� �ִϸ��̼� �ӵ�

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
        // ��ǥ Condition ����
        targetCondition = FindTargetCondition();

        // ��ǥ �� ����ȭ
        float normalizeValue = targetCondition.GetPercentage();
        targetFill = Mathf.Clamp01(normalizeValue);

        if (coroutine != null)
            StopCoroutine(coroutine);

        if (increaseValue <= 0)
        {
            // ����
            frontLayer.fillAmount = targetFill; // �� ���̾� ��� ����
            coroutine = StartCoroutine(DecreaseAnimation());
        }
        else
        {
            // ����
            backLayer.fillAmount = targetFill; // �� ������ ��� ����
            
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

        // ��� ���̾ �����ϰ� ����
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
            backLayer.fillAmount = Mathf.Lerp(startBack, targetFill, t); // �ε巴�� ����
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
