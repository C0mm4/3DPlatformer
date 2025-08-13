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
        // 사다리 탈 땐 사다리 이동 아니면 일반 이동
        if (isClimbing)
            ClimbMove();
        else
            Move();

    }

    private void Update()
    {
        // 공격 입력인 경우 무기 공격 진행
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


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLock, maxXlock);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // Move Key Input
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

    // Mouse Look Input
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Jump Key Input
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    // Attack Key Input
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            isAttackInput = true;
        }
        else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
        {
            isAttackInput = false;
        }
    }

    private bool isGrounded()
    {
        // 땅에 있는지 검사
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


    private void OnCollisionEnter(Collision collision)
    {
        // MovableObj 충돌 시 해당 객체에 힘 전달
        if (collision.transform.CompareTag("MovableObj"))
        {
            if(collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                var dot = Vector3.Dot(-collision.contacts[0].normal, this.body.velocity);
                body.AddForce(-collision.contacts[0].normal * dot, ForceMode.Impulse);
            }
        }

        // 사다리 충돌 시 사다리 모드
        if (collision.transform.CompareTag("Ladder"))
        {
            isClimbing = true;
            body.useGravity = false;
            body.velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 사다리 충돌 종료 시 사다리 모드 종료
        if (collision.transform.CompareTag("Ladder"))
        {
            isClimbing = false;
            body.useGravity = true;
        }
    }
}
