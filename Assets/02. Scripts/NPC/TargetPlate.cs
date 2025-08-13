using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TargetPlate : MonoBehaviour, IDamagable
{
    public GameObject DamageUI;

    public void TakePhysicalDamage(int damage)
    {
        // 맞을 시 UI만 생성
        CreateDamageUI(damage);
    }

    private void CreateDamageUI(int damage)
    {
        // UI객체를 생성
        GameObject go = Instantiate(DamageUI, transform.position, Quaternion.identity);
        var body = go.AddComponent<Rigidbody>();
        body.AddForce(Random.Range(-2, 2), Random.Range(5, 10), 0, ForceMode.Impulse);
        go.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        Destroy(go, 1f);
    }
}
