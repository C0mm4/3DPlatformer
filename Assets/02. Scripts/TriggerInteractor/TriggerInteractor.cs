using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerInteractor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEvent(other);
    }

    protected abstract void OnTriggerEvent(Collider collider);
}
