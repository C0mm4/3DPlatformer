using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : InteractorObj
{
    public ItemData data;

    public override string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public override void OnInteract()
    {
        if (data.type == ItemType.Consumable)
        {
            for (int i = 0; i < data.consumables.Length; i++)
            {
                switch (data.consumables[i].type)
                {
                    case ConsumableType.Health:
                        PlayerManager.Instance.Player.condition.AddHealth(data.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        PlayerManager.Instance.Player.condition.AddStamina(data.consumables[i].value);
                        break;
                }
            }
        }
        else if(data.type == ItemType.Equipable)
        {
            PlayerManager.Instance.Player.equipment.EquipNew(data);
        }
        else
        {
            Effect();
        }

        StartCoroutine(SetActiveLater(data.useCooltime));
    }

    private IEnumerator SetActiveLater(float time)
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        foreach(Transform trans in transform)
        {
            if(trans.TryGetComponent<Renderer>(out Renderer render))
            {
                render.enabled = false;
            }
            if (trans.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = false;
            }
        }

        yield return new WaitForSeconds(time);

        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        foreach (Transform trans in transform)
        {
            if (trans.TryGetComponent<Renderer>(out Renderer render))
            {
                render.enabled = true;
            }
            if (trans.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = true;
            }
        }
    }

    public virtual void Effect()
    {

    }
}
