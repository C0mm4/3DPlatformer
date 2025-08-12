using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLock;
    public float maxXlock;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("State")]
    public bool isClimbing = false;
    private bool isAttackInput;

    private Rigidbody body;

    private Equipment equip;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        equip = GetComponent<Equipment>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (isClimbing)
            ClimbMove();
        else
            Move();

        SlopeVelocityFix();
    }

    private void Update()
    {
        if (isAttackInput)
        {
            equip.OnAttackInput();
        }
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = body.velocity.y;

        body.velocity = dir;
    }

    void ClimbMove()
    {
        // 중력 제거
        body.useGravity = false;

        Vector3 dir = new Vector3(curMovementInput.x, curMovementInput.y, 0f) * moveSpeed;
        // 카메라 방향 기준 이동
        dir = transform.TransformDirection(dir);
        body.velocity = dir;
    }

    private void SlopeVelocityFix()
    {
        if (isGrounded() && !isClimbing)
        {
            Vector3 vel = body.velocity;
            if (vel.y > 0)
            {
                vel.y = 0;
                body.velocity = vel;
            }
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLock, maxXlock);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            isAttackInput = true;
//            equip.OnAttackInput();
        }
        else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
        {
            isAttackInput = false;
        }
    }

    private bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray((transform.position + (transform.forward * 0.2f) + transform.up * 0.01f), Vector3.down),
            new Ray((transform.position + (-transform.forward * 0.2f) + transform.up * 0.01f), Vector3.down),
            new Ray((transform.position + (transform.right * 0.2f) + transform.up * 0.01f), Vector3.down),
            new Ray((transform.position + (-transform.right * 0.2f) + transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("MovableObj"))
        {
            if(collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                var dot = Vector3.Dot(-collision.contacts[0].normal, this.body.velocity);
                body.AddForce(-collision.contacts[0].normal * dot, ForceMode.Impulse);
            }
        }


        if (collision.transform.CompareTag("Ladder"))
        {
            isClimbing = true;
            body.useGravity = false;
            body.velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ladder"))
        {
            isClimbing = false;
            body.useGravity = true;
        }
    }
}
