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
    /// Trigger �۵� �� ����Ǵ� �޼ҵ�
    /// </summary>
    /// <param name="collider">�浹 Collider</param>
    protected abstract void OnTriggerEvent(Collider collider);
}
