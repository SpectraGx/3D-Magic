using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    [Header("Inspector")]
    [SerializeField] private float speed = 10;
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource musicSource;

    private CharacterController characterController;
    private Vector3 currentMovent;
    private bool isMoving;
    private PlayerInput playerInput;
    private PlayerInteraction playerInteraction;
    Vector3 input;
    private bool isGamePaused = false;


    [Header("Animations")]
    private Animator animator;
    private string currentState;
    const string Player_Idle = "player_idle";
    const string Player_Walk = "player_walk";
    const string Player_walkObj = "player_walkObj";
    const string Player_Cut = "player_cutting";
    const string Player_IdleObj = "player_idleObj";


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        ControllerMovement();

        if (!isMoving && !playerInteraction.HasIngredientObject())
        {
            ChangeAnimationState(Player_Idle);
        }
        else if (isMoving && !playerInteraction.HasIngredientObject())
        {
            ChangeAnimationState(Player_Walk);
        }
        else if (!isMoving && playerInteraction.HasIngredientObject())
        {
            ChangeAnimationState(Player_IdleObj);
        }
        else if (isMoving && playerInteraction.HasIngredientObject())
        {
            ChangeAnimationState(Player_walkObj);
        }
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
            //ChangeAnimationState(Player_Walk);
            //moveParticles.Play();
        }
        else
        {
            isMoving = false;
            //moveParticles.Stop();
        }

        if (!isMoving)
        {
            //ChangeAnimationState(Player_Idle);
        }
    }

    public void Dash(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Dash");
        }
    }

    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
/*                 AudioSource[] audios = FindObjectsOfType<AudioSource>();
                foreach (AudioSource a in audios)
                {
                    a.Pause();
                } */
                musicSource.Pause();
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
/*                 AudioSource[] audios = FindObjectsOfType<AudioSource>();
                foreach (AudioSource a in audios)
                {
                    a.Play();
                } */
                musicSource.Play();
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    //  Metodo publico que indica si el personaje esta en movimiento o no
    public bool IsMoving() => isMoving;

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void ActivateParticles(bool activate)
    {
        if (moveParticles != null)
        {
            if (activate && !moveParticles.isPlaying)
            {
                moveParticles.Play();
            }
            else if (!activate && moveParticles.isPlaying)
            {
                moveParticles.Stop();
            }
        }
    }
}
