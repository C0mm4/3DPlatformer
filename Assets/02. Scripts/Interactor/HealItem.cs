using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        string str = $"Test";
        return str;
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Player.condition.AddHealth(5);
        Destroy(gameObject);
    }

}
