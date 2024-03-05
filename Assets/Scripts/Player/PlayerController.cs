using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //          VARIABLES           // 
    [SerializeField] private float speed = 4;

    private CharacterController characterController;    //Referencia a CharacterController
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 input;
    private Vector3 currentMovent;
    private bool isMoving;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        ControllerMovement();
        //input = playerInput.actions["Move"].ReadValue<Vector2>(); 
    }

    private void FixedUpdate()
    {
        //rb.AddForce(new Vector3(input.x,0f,input.y)*speed);
    }

    private void ControllerMovement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        currentMovent = move.normalized * (Time.deltaTime * speed);
        characterController.Move(currentMovent);

        // Control de la rotacion del personaje
        if (move != Vector3.zero)
        {
            isMoving = true;
            Quaternion toRotation = Quaternion.LookRotation(currentMovent);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 2000 * Time.deltaTime);
        }
        else
        {
            isMoving = true;
        }
    }

    public void Cut(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Esta cortando");
        }
    }

    public bool IsMoving() => isMoving;
}
