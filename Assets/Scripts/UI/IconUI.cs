using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IconUI : MonoBehaviour
{
    [SerializeField] private Cauldron cauldron; // Referencia al script del caldero
    [SerializeField] private Transform iconTemplate; // Plantilla del icono de ingrediente

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false); // Desactiva la plantilla en el inicio
    }

    private void Start()
    {
        cauldron.OnProgressChanged += UpdateVisual; // Suscribe al evento de cambio de progreso
    }

    private void UpdateVisual(object sender, UIProgress.OnProgressChangedEventArgs e)
    {
        // Elimina todos los iconos existentes
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // Itera a través de los ingredientes en el caldero y muestra sus íconos
        foreach (IngredientData ingredientData in cauldron.GetCauldronIngredientDataList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<IconSingleUI>().SetIngredientObjectData(ingredientData);
        }
    }
}
