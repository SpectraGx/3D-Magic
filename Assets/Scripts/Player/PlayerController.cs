using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //          VARIABLES           // 
    [SerializeField] private float speed = 4;     //Control de velocidad

    private CharacterController characterController;    //Referencia a CharacterController
    private Vector3 currentMovent;  // Dirección y magnitud del movimiento
    private bool isMoving;      // Booleano que indica si el personaje se mueve

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();  // Referencia a CharacterController
    }

    private void Update()
    {
        ControllerMovement();
    }

    private void ControllerMovement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));    // Entradas de teclado
        currentMovent = move.normalized * (Time.deltaTime * speed);     // Normaliza la velocidad por el tiempo
        characterController.Move(currentMovent);    // Aplica movimiento

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

    //  Metodo publico que indica si el personaje esta en movimiento o no
    public bool IsMoving() => isMoving;
}
