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

        // 작동 시 이동
        if (isTrigger)
        {
            transform.position += Vector3.forward * Time.fixedDeltaTime;
        } 

        // 움직인 값 계산
        Vector3 delta = transform.position - lastPosition;

        // 접촉한 물체들의 위치 조정
        foreach (var rb in passengers)
        {
            if (rb != null)
            {
                rb.position += delta; 
            }
        }
        // 마지막 위치 갱신
        lastPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 해당 RigidBody 등록
        Rigidbody rb = collision.rigidbody;
        if (rb != null && !passengers.Contains(rb))
        {
            passengers.Add(rb);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 충돌 해제 시 등록 해제
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
