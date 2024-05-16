using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play(string name)
    {
        SceneManager.LoadScene(name);
        Debug.Log(name);
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Salir");
    }

    public void FullScreen(){
        Screen.fullScreen = true;
    }

    public void Window(){
        Screen.fullScreen = false;
        Screen.SetResolution(1280, 720, false);
    }
}
