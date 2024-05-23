using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider myMusicSlider;
    [SerializeField] private Slider mySfxSlider;

    private void Start()
    {

        if (PlayerPrefs.HasKey("musicVolumen"))
        {
            LoadVolumen();
        }
        else
        {
            SetMusicVolumen();
            SetSfxVolumen();
        }


    }

    public void SetMusicVolumen()
    {
        float volumen = myMusicSlider.value;
        if (volumen == 0)
        {
            myMixer.SetFloat("Music", -80);
        }
        else
        {
            myMixer.SetFloat("Music", Mathf.Log10(volumen) * 20);
        }
        PlayerPrefs.SetFloat("musicVolumen", volumen);
    }

    public void SetSfxVolumen()
    {

        float volumen = mySfxSlider.value;
        if (volumen == 0)
        {
            myMixer.SetFloat("Sonido", -80);
        }
        else
        {
            myMixer.SetFloat("Sonido", Mathf.Log10(volumen) * 20);
        }
        PlayerPrefs.SetFloat("sonidoVolumen", volumen);
    }

    private void LoadVolumen()
    {
        myMusicSlider.value = PlayerPrefs.GetFloat("musicVolumen");
        SetMusicVolumen();
        mySfxSlider.value = PlayerPrefs.GetFloat("sonidoVolumen");
        SetSfxVolumen();

    }
}
