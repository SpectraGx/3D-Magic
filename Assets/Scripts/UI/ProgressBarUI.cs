using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressGameObject;
    [SerializeField] private Image barImage;

    private UIProgress uiProgress;

    private void Start() {
        uiProgress = progressGameObject.GetComponent<UIProgress>();
        if (uiProgress == null) {
            Debug.LogError("El objeto " + progressGameObject + " no tiene un componente que implemente UIProgress");
        }

        uiProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, UIProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
