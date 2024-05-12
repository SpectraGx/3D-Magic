using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform potionTemplate;

    private void Awake()
    {
        potionTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnPotionSpawned += DeliveryManager_OnPotionSpawned; 
        DeliveryManager.Instance.OnPotionCompleted += DeliveryManager_OnPotionCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnPotionCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnPotionSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == potionTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (IngredientData ingredientData in DeliveryManager.Instance.GetWaitingPotionDataList())
        {
            Transform recipeTransform = Instantiate(potionTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            //recipeTransform.Find("PotionNameText");
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetPotionData(ingredientData);
        }
    }
}
