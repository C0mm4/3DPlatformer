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
        // 사용 아이템일 시 사용
        if (data.type == ItemType.Consumable)
        {
            for (int i = 0; i < data.consumables.Length; i++)
            {
                // Consumables의 타입에 따라 작용
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
            // 장착 아이템일 경우 장착
            PlayerManager.Instance.Player.equipment.EquipNew(data);
        }
        else
        {
            // 사용, 장착 아닐 시 (EffectItem) 일 경우 Effect 실행
            Effect();
        }
        // Cool Time 후 재활성화
        StartCoroutine(SetActiveLater(data.useCooltime));
    }

    private IEnumerator SetActiveLater(float time)
    {
        // 오브젝트와 자식 오브젝트의 Renderer, collider 비활성화
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

        // Renderer, Collider 활성화
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
        // 오버라이드 해서 추가 이펙트 구현
    }
}
