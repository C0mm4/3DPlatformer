using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetPlate : MonoBehaviour, IDamagable
{
    public GameObject DamageUI;

    public void TakePhysicalDamage(int damage)
    {
        Debug.Log("Hit!");
        CreateDamageUI(damage);
    }

    private void CreateDamageUI(int damage)
    {
        GameObject go = Instantiate(DamageUI, transform.position, Quaternion.identity);
        var body = go.AddComponent<Rigidbody>();
        body.AddForce(Random.Range(-2, 2), Random.Range(5, 10), 0, ForceMode.Impulse);
        Destroy(go, 1f);
    }
}
