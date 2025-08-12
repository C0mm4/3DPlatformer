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
        condition.onChangeHP += HPChangeAction;
        condition.onChangeHP += HPBar.OnChangeValue;
        condition.onChangeStamina += StaminaChangeAction;
        condition.onChangeStamina += StaminaBar.OnChangeValue;
        GetComponent<Interaction>().OnInteractionCheckByRay += EnterInteractRay;
        GetComponent<Interaction>().ExitInteractionCheckByRay += ExitInteractRay;

    }

    public void OnDisable()
    {
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

        StatIndicator.gameObject.SetActive(true);
        if(value < 0)
        {
            StatIndicator.color = new Color(1f, 100f / 255f, 100f / 255f);
        }
        else if (value > 0)
        {
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

        if (value <= 25) return;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        StatIndicator.color = new Color(8f / 255f, 255f / 255f, 202f / 255f);
        
        StatIndicator.gameObject.SetActive(true);
        coroutine = StartCoroutine(IndicatorAlphaDecrease());
    }

    public IEnumerator IndicatorAlphaDecrease(float startAlpha = 0.3f)
    {
        float a = startAlpha;

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
        InteractorText.gameObject.SetActive(true);
        InteractorText.text = text;
    }

    public void ExitInteractRay()
    {
        InteractorText.gameObject.SetActive(false);
    }
}
