using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] private float speed = 10;
    [SerializeField] private ParticleSystem moveParticles;

    private CharacterController characterController;
    private Vector3 currentMovent;
    private bool isMoving;
    private PlayerInput playerInput;
    Vector3 input;

    [Header("Animations")]
    private Animator animator;
    private string currentState;
    const string Player_Idle = "player_idle";
    const string Player_Walk = "player_walk";
    const string Player_walkObj = "player_walkObj";
    const string Player_Cut = "player_cutting";


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
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
            Quaternion toRotation = Quaternion.LookRotation(-currentMovent);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 2000 * Time.deltaTime);
            ChangeAnimationState(Player_Walk);
            //moveParticles.Play();
        }
        else
        {
            isMoving = false;
            //moveParticles.Stop();
        }

        if (!isMoving){
            ChangeAnimationState(Player_Idle);
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

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void ActivateParticles(bool activate){
        if (moveParticles != null){
            if (activate && !moveParticles.isPlaying){
                moveParticles.Play();
            }
            else if (!activate && moveParticles.isPlaying){
                moveParticles.Stop();
            }
        }
    }
}
