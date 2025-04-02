using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int playerIndex = 0;
    [Header("Movement Speed")]
    [SerializeField] 
    private float speed = 5f;
    [Header("Movement Slope")]
    [SerializeField]
    private float slope = 40f;
    [Header("References")]
    [SerializeField] 
    private GameObject avatar;
    [SerializeField] 
    private InputActionAsset actionAsset;
    [SerializeField] 
    private Animator animator;
    
    private PlayerInput playerInput;
    private Rigidbody rb;
    
    private RaycastHit slopeHit;
    private Vector2 move;
    private Vector2 rotateDirection;

    private Target target;
    private bool canMove = true;
    private bool isImpulsed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        target = GetComponent<Target>();
        target.onDeath.AddListener(HandleDeath);
        target.onRevive.AddListener(HandleRevive);
    }
    private void HandleRevive()
    {
        canMove = true;
    }

    private void HandleDeath(PlayerStatistics shootingPlayer)
    {
        canMove = false;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        
        Vector3 desiredVelocity = Vector3.zero;
        if (move != Vector2.zero && playerIndex == playerInput.playerIndex)
        {
            desiredVelocity = new Vector3(move.x, 0, move.y) * speed;
        }
        
        if (!IsGrounded() && !OnSlope())
        {
            rb.AddForce(desiredVelocity, ForceMode.Force);
        }
        else if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection(desiredVelocity.normalized) * (speed * 10f), ForceMode.Force);
        }
        else
        {
            rb.AddForce(desiredVelocity * 10f, ForceMode.Force);
        }
        
    }

    void Update()
    {
        if (!canMove)
            return;
        
        ModifyDragWhenGrounded();
        
        if (rotateDirection != Vector2.zero && playerIndex == playerInput.playerIndex)
        {
            avatar.transform.forward = new Vector3(rotateDirection.x, 0, rotateDirection.y);
        }
        HandleAnimation();
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        rotateDirection = context.ReadValue<Vector2>();
    }

    public void SetPlayerLayer(int layerToAdd)
    {
        gameObject.layer = layerToAdd;
        GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
    }

    public void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        rb.MovePosition(position);
        rb.MoveRotation(rotation);
    }
    
    void OnDisable()
    {
        target.onDeath.AddListener(HandleDeath);
    }

    public void AddImpact(Vector3 direction, float forceImpulse)
    {
        rb.AddForce(direction * forceImpulse, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < slope && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction.normalized, slopeHit.normal).normalized;
    }

    private void ModifyDragWhenGrounded()
    {
        rb.drag = IsGrounded() ? 5f : 0f;
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.05f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.black : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }

    private void HandleAnimation()
    {
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0f;
        animator.SetFloat("Speed", currentVelocity.magnitude);
        animator.SetBool("IsGrounded", IsGrounded());
        Vector3 velocityWithAvatarRotation = avatar.transform.InverseTransformDirection(currentVelocity);
        Debug.Log(velocityWithAvatarRotation);
        animator.SetFloat("Right",velocityWithAvatarRotation.x);
        animator.SetFloat("Forward", velocityWithAvatarRotation.z);
    }
}