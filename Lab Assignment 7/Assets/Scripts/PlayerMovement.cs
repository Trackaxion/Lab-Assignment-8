using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerInput;
    private CharacterController characterController;
    private Vector3 playerDirection;
    private Camera mainCamera;
    private float gravity = -9.81f;
    private float velocity;
    [SerializeField] private float gravityMultiplier = 3f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float speed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity < 0f)
        {
            velocity = -1f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
       
        playerDirection.y = velocity;
    }

    private void ApplyRotation()
    {
        if (playerInput.sqrMagnitude == 0) return;
        playerDirection = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f) * new Vector3(playerInput.x, 0f, playerInput.y);
        var targetRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        characterController.Move(playerDirection * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
        playerDirection = new Vector3(playerInput.x, 0f, playerInput.y);
    }
}
