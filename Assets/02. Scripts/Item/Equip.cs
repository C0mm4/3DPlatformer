using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    public float useStamina;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    [Header("Effect")]
    [SerializeField]
    private ParticleSystem particle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public void OnAttack()
    {
        if (!attacking)
        {
            if (PlayerManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
                particle.Play();
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {

        }
    }
}
