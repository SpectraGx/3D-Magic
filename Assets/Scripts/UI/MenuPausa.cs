using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    [SerializeField]private GameManager gameManager;
    public void Play(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Reanude(){
        gameManager.Pause();
    }
}
