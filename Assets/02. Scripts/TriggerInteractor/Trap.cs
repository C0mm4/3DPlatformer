using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TriggerInteractor
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTriggerEvent(Collider collider)
    {
        animator.SetTrigger("open");
    }


}
