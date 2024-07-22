using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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


    private Vector2 move;
    private Vector2 rotateDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (move is not { x: 0, y: 0 } && playerIndex == playerInput.playerIndex)
        {
            Vector3 moveDirection = new Vector3(move.x, 0, move.y);
            characterController.Move(moveDirection * Time.deltaTime * speed);
        }

        if (rotateDirection is not { x: 0, y: 0 } && playerIndex == playerInput.playerIndex)
        {
            avatar.transform.forward = new Vector3(rotateDirection.x, 0, rotateDirection.y);
        }
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
}
