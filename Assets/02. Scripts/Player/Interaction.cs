using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject currentInteractObj;
    private IInteractable curInteractable;

    private Camera camera;

    public Action<string> OnInteractionCheckByRay;
    public Action ExitInteractionCheckByRay;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != currentInteractObj)
                {
                    currentInteractObj = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    OnInteractionCheckByRay?.Invoke(curInteractable.GetInteractPrompt());
                }
            }
            else
            {
                currentInteractObj = null;
                curInteractable = null;
                ExitInteractionCheckByRay?.Invoke();
            }
        }
    }


    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            currentInteractObj = null;
            curInteractable = null;
        }
    }
}
