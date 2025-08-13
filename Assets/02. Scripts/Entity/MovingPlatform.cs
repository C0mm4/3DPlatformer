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

        // �۵� �� �̵�
        if (isTrigger)
        {
            transform.position += Vector3.forward * Time.fixedDeltaTime;
        } 

        // ������ �� ���
        Vector3 delta = transform.position - lastPosition;

        // ������ ��ü���� ��ġ ����
        foreach (var rb in passengers)
        {
            if (rb != null)
            {
                rb.position += delta; 
            }
        }
        // ������ ��ġ ����
        lastPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �浹 �� �ش� RigidBody ���
        Rigidbody rb = collision.rigidbody;
        if (rb != null && !passengers.Contains(rb))
        {
            passengers.Add(rb);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �浹 ���� �� ��� ����
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
