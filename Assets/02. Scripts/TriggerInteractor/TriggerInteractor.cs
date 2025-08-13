using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerInteractor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEvent(other);
    }

    /// <summary>
    /// Trigger 작동 시 적용되는 메소드
    /// </summary>
    /// <param name="collider">충돌 Collider</param>
    protected abstract void OnTriggerEvent(Collider collider);
}
