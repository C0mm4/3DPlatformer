using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractorObj : MonoBehaviour, IInteractable
{
    public string PromptText;

    public virtual string GetInteractPrompt()
    {
        return PromptText;
    }

    public abstract void OnInteract();

}
