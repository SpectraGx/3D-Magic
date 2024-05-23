using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Variables")]
    private int width, height;
    private int newResolution;
    private bool screnF;
    private int controlsNum;
    [SerializeField] private TextMeshProUGUI resolutioonText;
    private Dictionary<GameObject, GameObject> panelFirstButtonMapping;

    [Header("Paneles")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject controlsKeyboardMenu;
    [SerializeField] private GameObject controlsControlsMenu;
    [SerializeField] private GameObject controlsHow;
    [SerializeField] private GameObject videoMenu;
    [SerializeField] private GameObject creditsMenu;


    [Header("Botones Iniciales")]
    [SerializeField] private GameObject mainFirstButton;
    [SerializeField] private GameObject playFirstButton;
    [SerializeField] private GameObject optionsFirstButton;
    [SerializeField] private GameObject audioFirstButton;
    [SerializeField] private GameObject controlsFirstButton;
    [SerializeField] private GameObject videoFirstButton;
    [SerializeField] private GameObject creditsFirstButton;


    private void Awake()
    {
        panelFirstButtonMapping = new Dictionary<GameObject, GameObject>(){
            {mainMenu, mainFirstButton},
            {playMenu, playFirstButton},
            {optionsMenu, optionsFirstButton},
            {audioMenu, audioFirstButton},
            {controlsMenu, controlsFirstButton},
            {videoMenu, videoFirstButton},
            {creditsMenu, creditsFirstButton},
        };

        LoadUserSettings();

    }

    private void LoadUserSettings()
    {
        // Cargar el modo de pantalla completa
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            screnF = PlayerPrefs.GetInt("fullscreen") == 1;
            Screen.fullScreen = screnF;
        }

        // Cargar la resolución
        if (PlayerPrefs.HasKey("resolutionWidth") && PlayerPrefs.HasKey("resolutionHeight"))
        {
            width = PlayerPrefs.GetInt("resolutionWidth");
            height = PlayerPrefs.GetInt("resolutionHeight");
            Screen.SetResolution(width, height, screnF);
        }

        // Cargar el índice de la resolución
        if (PlayerPrefs.HasKey("newResolution"))
        {
            newResolution = PlayerPrefs.GetInt("newResolution");
            Resolutions();
        }
    }

    public void Play(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void FullScreen()
    {
        Screen.fullScreen = true;
        screnF = true;
        PlayerPrefs.SetInt("FullScreen", 1);
    }

    public void Window()
    {
        Screen.fullScreen = false;
        screnF = false;
        PlayerPrefs.SetInt("FullScreen", 0);
    }

    public void OpenPanel(GameObject panelToOpen)
    {
        foreach (var panel in panelFirstButtonMapping.Keys)
        {
            panel.SetActive(false);
        }

        panelToOpen.SetActive(true);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (panelFirstButtonMapping.TryGetValue(panelToOpen, out GameObject firstButton))
            {
                EventSystem.current.SetSelectedGameObject(firstButton);
            }
        }
    }

    public void OpenMainMenu()
    {
        OpenPanel(mainMenu);
    }

    public void OpenPlay()
    {
        OpenPanel(playMenu);
    }

    public void OpenOptions()
    {
        OpenPanel(optionsMenu);
    }

    public void OpenAudio()
    {
        OpenPanel(audioMenu);
    }

    public void OpenControls()
    {
        OpenPanel(controlsMenu);
    }

    public void NextControl()
    {
        controlsNum++;
        ControlsPanel();
    }

    public void BackControl()
    {
        controlsNum--;
        ControlsPanel();
    }

    public void ControlsPanel()
    {
        /* if (controlsKeyboardMenu.activeSelf)
        {
            controlsKeyboardMenu.SetActive(false);
            controlsControlsMenu.SetActive(true);
        }
        else if (controlsControlsMenu.activeSelf)
        {
            controlsControlsMenu.SetActive(false);
            controlsKeyboardMenu.SetActive(true);
        } */
        controlsNum = Mathf.Clamp(controlsNum, 0, 2);
        switch (controlsNum)
        {
            case 0:
                controlsKeyboardMenu.SetActive(false);
                controlsHow.SetActive(false);
                controlsControlsMenu.SetActive(true);
                break;
            case 1:
                controlsKeyboardMenu.SetActive(false);
                controlsHow.SetActive(true);
                controlsControlsMenu.SetActive(false);
                break;
            case 2:
                controlsKeyboardMenu.SetActive(true);
                controlsHow.SetActive(false);
                controlsControlsMenu.SetActive(false);
                break;
        }

    }

    public void OpenVideo()
    {
        OpenPanel(videoMenu);
    }

    public void NextResolution()
    {
        newResolution++;
        Resolutions();
    }

    public void BackResolution()
    {
        newResolution--;
        Resolutions();
    }

    public void ApplyResolution()
    {
        Screen.SetResolution(width, height, screnF);
        PlayerPrefs.SetInt("resolutionWidth", width);
        PlayerPrefs.SetInt("resolutionHeight", height);
        PlayerPrefs.SetInt("resolution", newResolution);
        PlayerPrefs.Save();
    }

    private void Resolutions()
    {
        newResolution = Mathf.Clamp(newResolution, 0, 3);
        switch (newResolution)
        {
            case 0:
                width = 1024;
                height = 576;
                break;
            case 1:
                width = 1280;
                height = 720;
                break;
            case 2:
                width = 1366;
                height = 768;
                break;
            case 3:
                width = 1920;
                height = 1080;
                break;
        }
        resolutioonText.text = width.ToString() + " - " + height.ToString();
    }

    public void OpenCredits()
    {
        OpenPanel(creditsMenu);
    }

    public void URL(string url)
    {
        Application.OpenURL(url);
    }
}
