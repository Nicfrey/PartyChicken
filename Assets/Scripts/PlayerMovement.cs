using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int playerIndex = 0;
    private PlayerInput playerInput;
    private CharacterController characterController;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject avatar;

    private bool isGrounded = false;

    [SerializeField] 
    private float gravity;
    private Vector3 gravityImpulse;
    private Vector2 move;
    private Vector2 rotateDirection;

    private Target target;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        target = GetComponent<Target>();
        target.onDeath += HandleDeath;
        target.onRevive += HandleRevive;
    }
    private void HandleRevive()
    {
        canMove = true;
    }

    private void HandleDeath()
    {
        canMove = false;
    }

    void Update()
    {
        if (!canMove)
            return;

        HandledGrounded();

        Vector3 velocity = new Vector3(0,0,0);
        if (move != Vector2.zero && playerIndex == playerInput.playerIndex)
        {
            velocity = new Vector3(move.x, 0, move.y) * speed;
        }

        if (rotateDirection != Vector2.zero && playerIndex == playerInput.playerIndex)
        {
            avatar.transform.forward = new Vector3(rotateDirection.x, 0, rotateDirection.y);
        }

        if (!isGrounded)
        {
            gravityImpulse.y -= gravity * Time.deltaTime;
        }
        velocity += gravityImpulse;
        Debug.Log("Velocity Player " + playerIndex + ": " + velocity);
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        rotateDirection = context.ReadValue<Vector2>();
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetPlayerLayer(int layerToAdd)
    {
        gameObject.layer = layerToAdd;
        GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
    }

    public void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        characterController.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        characterController.enabled = true;
    }

    public void AddImpact(Vector3 direction, float force)
    {
        gravityImpulse = direction.normalized * force;
    }

    private void HandledGrounded()
    {
        bool previousGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        if (isGrounded != previousGrounded && isGrounded)
        {
            gravityImpulse = Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, characterController.height / 2 + 0.1f))
        {
            Gizmos.DrawSphere(hit.point,0.05f);
            Gizmos.DrawRay(transform.position, Vector3.down * 0.1f);
        }
    }

    void OnDisable()
    {
        target.onDeath -= HandleDeath;
    }
}