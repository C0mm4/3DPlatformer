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
            int objLayerMask = 1 << hit.collider.gameObject.layer;

            if ((bulletHoleLayer.value & objLayerMask) != 0)
            {
                CreateBulletHole(hit);
            }

            if (hit.collider.CompareTag("MovableObj"))
            {
                if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    var dot = Vector3.Dot(-hit.normal, ray.direction * damage * 4);
                    body.AddForce(-hit.normal * dot, ForceMode.Impulse);
                }
            }

            if(hit.collider.TryGetComponent<IDamagable>(out IDamagable instance))
            {
                instance.TakePhysicalDamage(this.damage);
            }

            Debug.Log("Hit: " + hit.collider.name);
        }
    }
    void CreateBulletHole(RaycastHit hit)
    {
        // 탄흔 생성
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);

        // 벽 표면 방향으로 회전
        bulletHole.transform.rotation = Quaternion.LookRotation(-hit.normal);

        // 탄흔을 맞은 오브젝트에 종속시켜서 같이 움직이게
        bulletHole.transform.SetParent(hit.collider.transform);

        // 일정 시간 후 삭제 (예: 10초 뒤)
        Destroy(bulletHole, 10f);
    }
}
