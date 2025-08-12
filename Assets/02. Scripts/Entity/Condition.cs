using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Condition
{
    public float currentValue;
    public float startValue;
    public float maxValue;

    public void Start()
    {
        currentValue = startValue;
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }

    public void Add(float value)
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }

    public void Subtract(float value)
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }
}
