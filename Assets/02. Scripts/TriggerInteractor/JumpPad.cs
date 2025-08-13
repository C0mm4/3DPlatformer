using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : TriggerInteractor
{
    public float jumpForce;
    protected override void OnTriggerEvent(Collider collider)
    {
        // body가 있는 객체면 점프시킴
        if(collider.TryGetComponent<Rigidbody>(out Rigidbody body))
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
