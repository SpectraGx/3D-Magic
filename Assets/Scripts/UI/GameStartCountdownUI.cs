using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI startText;
    //[SerializeField] private string phraseText;
    [SerializeField] private float timePhraseText = 1f;
    [SerializeField] private AudioClip countdownAudio;

    private Animator animator;
    private AudioSource audioSource;
    private int previousCountdownNumber;
    private const string NUMBER_POPUP = "NumberPopup";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
        startText.gameObject.SetActive(false);
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
            PlayCountdownAudio();
        }
        else
        {
            Hide();
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            StartCoroutine(ShowMessageForSeconds(timePhraseText));
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
        }
    }

    private void Show()
    {
        countdownText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }

    private IEnumerator ShowMessageForSeconds(float duration)
    {
        animator.SetTrigger(NUMBER_POPUP);
        //startText.text = message;
        startText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        startText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void PlayCountdownAudio()
    {
        if (countdownAudio != null && audioSource != null)
        {
            audioSource.clip = countdownAudio;
            audioSource.Play();
        }
    }
}
