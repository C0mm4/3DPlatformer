using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : TriggerInteractor
{
    public float jumpForce;
    protected override void OnTriggerEvent(Collider collider)
    {
        if(collider.TryGetComponent<Rigidbody>(out Rigidbody body))
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
