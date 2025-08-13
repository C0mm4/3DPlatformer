using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header("Stat Indicator")]
    public Image StatIndicator;
    public float IndicateTime;

    Coroutine coroutine;

    public BarUI HPBar;
    public BarUI StaminaBar;

    [Header("Interaction UI")]
    public TextMeshProUGUI InteractorText;

    private PlayerCondition condition;
    private Interaction interaction;

    private void Awake()
    {
        condition = GetComponent<PlayerCondition>();
        interaction = GetComponent<Interaction>();

    }


    public void OnEnable()
    {
        // Action 구독
        condition.onChangeHP += HPChangeAction;
        condition.onChangeHP += HPBar.OnChangeValue;
        condition.onChangeStamina += StaminaChangeAction;
        condition.onChangeStamina += StaminaBar.OnChangeValue;
        GetComponent<Interaction>().OnInteractionCheckByRay += EnterInteractRay;
        GetComponent<Interaction>().ExitInteractionCheckByRay += ExitInteractRay;

    }

    public void OnDisable()
    {
        // Action 해제
        condition.onChangeHP -= HPChangeAction;
        condition.onChangeHP -= HPBar.OnChangeValue;
        condition.onChangeStamina -= StaminaChangeAction;
        condition.onChangeStamina -= StaminaBar.OnChangeValue;
        interaction.OnInteractionCheckByRay -= EnterInteractRay;
        interaction.ExitInteractionCheckByRay -= ExitInteractRay;
    }

    public void HPChangeAction(float value)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // Stat Indicator 색상 설정
        StatIndicator.gameObject.SetActive(true);
        if(value < 0)
        {
            // 감소
            StatIndicator.color = new Color(1f, 100f / 255f, 100f / 255f);
        }
        else if (value > 0)
        {
            // 회복
            StatIndicator.color = new Color(8f / 255f, 255f / 255f, 53f / 255f);
        }
        else
        {
            // HP change value is 0, when call it initialize
            StatIndicator.color = new Color(8f / 255f, 255f / 255f, 53f / 255f, 0);
            coroutine = StartCoroutine(IndicatorAlphaDecrease(0));
            return;
        }
        coroutine = StartCoroutine(IndicatorAlphaDecrease());
    }

    public void StaminaChangeAction(float value) 
    {
        // 크게 증가하는 거 아니면 무시
        if (value <= 25) return;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        // Stat Indicator 색상 설정
        StatIndicator.color = new Color(8f / 255f, 255f / 255f, 202f / 255f);
        
        StatIndicator.gameObject.SetActive(true);
        coroutine = StartCoroutine(IndicatorAlphaDecrease());
    }

    public IEnumerator IndicatorAlphaDecrease(float startAlpha = 0.3f)
    {
        float a = startAlpha;

        // 알파 점차 감소
        while (a > 0)
        {
            a -= (startAlpha / IndicateTime) * Time.deltaTime;
            StatIndicator.SetUIImageAlpha(a);
            yield return null;
        }

        StatIndicator.gameObject.SetActive(false);
    }

    public void EnterInteractRay(string text)
    {
        // 상호작용 UI 활성화
        InteractorText.gameObject.SetActive(true);
        InteractorText.text = text;
    }

    public void ExitInteractRay()
    {
        // 상호작용 UI 비활성화
        InteractorText.gameObject.SetActive(false);
    }
}
