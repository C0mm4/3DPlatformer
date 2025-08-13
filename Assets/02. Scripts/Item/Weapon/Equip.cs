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
    public LayerMask bulletHoleLayer;
    public GameObject bulletHolePrefab;

    private Animator animator;
    private Camera camera;
    

    [Header("Effect")]
    [SerializeField]
    private ParticleSystem particle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1 / attackRate;
        camera = Camera.main;
    }

    public void OnAttack()
    {
        if (!attacking)
        {
            if (PlayerManager.Instance.Player.condition.UseStamina(useStamina))
            {
                // 공격 중이 아니고, 스태미나가 남아있으면 공격
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
            // 탄흔이 남을 수 있는 오브젝트면 탄흔 생성
            int objLayerMask = 1 << hit.collider.gameObject.layer;

            if ((bulletHoleLayer.value & objLayerMask) != 0)
            {
                CreateBulletHole(hit);
            }

            // MovableObj일 경우 해당 방향으로 힘 추가
            if (hit.collider.CompareTag("MovableObj"))
            {
                if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    var dot = Vector3.Dot(-hit.normal, ray.direction * damage * 4);
                    body.AddForce(-hit.normal * dot, ForceMode.Impulse);
                }
            }

            // 데미지 받는 객체면 데미지 부여
            if(hit.collider.TryGetComponent<IDamagable>(out IDamagable instance))
            {
                instance.TakePhysicalDamage(this.damage);
            }

            Debug.Log("Hit: " + hit.collider.name);
        }
    }
    void CreateBulletHole(RaycastHit hit)
    {
        // 탄흔 생성 및 위치 조정
        var bulletGo = ObjectPoolManager.Instance.Pool.Get();
        bulletGo.transform.position = hit.point + hit.normal * 0.001f;

        // 벽 표면 방향으로 회전
        bulletGo.transform.rotation = Quaternion.LookRotation(-hit.normal);

        // 부모를 해당 오브젝트로 하여 같이 움직이게
        bulletGo.transform.SetParent(hit.collider.transform);

    }
}
