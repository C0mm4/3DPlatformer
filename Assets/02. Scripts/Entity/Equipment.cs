using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    public PlayerController controller;
    public PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        Unequip();
        curEquip = Instantiate(data.equipPrefab, equipParent, false).GetComponent<Equip>();

    }

    public void Unequip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput()
    {
        if (curEquip != null)
        {
            curEquip.OnAttack();
        }
    }
}
