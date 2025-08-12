using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : InteractorObj
{
    private Vector3 lastPosition;
    private List<Rigidbody> passengers = new List<Rigidbody>();
    private bool isTrigger = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 delta = transform.position - lastPosition;

        foreach (var rb in passengers)
        {
            if (rb != null)
            {
                rb.position += delta; // 플랫폼 이동량만큼 같이 이동
            }
        }


        lastPosition = transform.position;
        if (isTrigger)
        {
            transform.position += Vector3.forward * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null && !passengers.Contains(rb))
        {
            passengers.Add(rb);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            passengers.Remove(rb);
        }
    }


    public override void OnInteract()
    {
        if (!isTrigger)
        {
            gameObject.layer = default;
            isTrigger = true;
        }
    }
}
