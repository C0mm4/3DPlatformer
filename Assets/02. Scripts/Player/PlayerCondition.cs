using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition HP;
    public Condition Stamina;

    public event Action<float> onChangeHP;
    public event Action<float> onChangeStamina;

    private void Awake()
    {
        HP.Start();
        Stamina.Start();
    }

    private void Start()
    {
        // UI 갱신을 위한 더미 값 증감
        AddHealth(0);
        AddStamina(0);
    }

    public void AddHealth(float value)
    {
        HP.Add(value);
        onChangeHP?.Invoke(value);
    }

    public void DecreaseHealth(float value)
    {
        HP.Subtract(value);
        onChangeHP?.Invoke(-value);
    }

    public void AddStamina(float value)
    {
        Stamina.Add(value);
        onChangeStamina?.Invoke(value);
    }

    public bool UseStamina(float value)
    {
        if (Stamina.currentValue - value < 0)
        {
            return false;
        }

        Stamina.Subtract(value);
        onChangeStamina?.Invoke(-value);
        return true;
    }

    public void Update()
    {
        /* Test Block
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddHealth(5f);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddStamina(26f);
        }
        */
    }

}
