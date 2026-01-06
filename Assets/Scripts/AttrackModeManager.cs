using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AttractModeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToWait = 300f; 
    
    [Header("Reference")]
    [SerializeField] private GameObject mainMenuContainer; 
    [SerializeField] private RawImage videoScreen;         
    [SerializeField] private VideoPlayer videoPlayer;      

    private float lastInputTime;
    private bool isAttractModeActive = false;

    private void Start()
    {
        lastInputTime = Time.time;
        
        videoPlayer.renderMode = VideoRenderMode.APIOnly;
        videoPlayer.prepareCompleted += OnVideoPrepared;
        
        videoScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void Update()
    {
        if (isAttractModeActive) return;

        if (Time.time - lastInputTime >= timeToWait)
        {
            StartAttractMode();
        }
    }

    // --- DETECCIÓN DE INPUT ---
    
    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

        
        lastInputTime = Time.time;

        if (isAttractModeActive)
        {
            StopAttractMode();
        }
    }


    private void StartAttractMode()
    {
        isAttractModeActive = true;

        videoScreen.gameObject.SetActive(true);

        videoScreen.enabled = false;
        
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        if (!isAttractModeActive) return; 

        mainMenuContainer.SetActive(false);
        
        videoScreen.texture = videoPlayer.texture;

        videoScreen.enabled = true;
        
        videoPlayer.Play();
    }

    private void StopAttractMode()
    {
        isAttractModeActive = false;

        videoPlayer.Stop();
        
        videoScreen.gameObject.SetActive(false);

        mainMenuContainer.SetActive(true);
    }
}