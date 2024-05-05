using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed = 10;

    private CharacterController characterController;
    private Vector3 currentMovent;
    private bool isMoving;
    private PlayerInput playerInput;
    Vector3 input;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        ControllerMovement();

    }

    private void ControllerMovement()
    {
        Vector3 move = new Vector3(input.x, 0, input.y);    // Entradas de teclado
        currentMovent = move * (Time.deltaTime * speed);    // Normaliza la velocidad por el tiempo
        characterController.Move(currentMovent);            // Aplica movimiento

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

    public void Dash(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Dash");
        }
    }

    //  Metodo publico que indica si el personaje esta en movimiento o no
    public bool IsMoving() => isMoving;
}
