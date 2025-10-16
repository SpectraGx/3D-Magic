using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private Image iconPotion;

    public void SetPotionData(IngredientData potionData)
    {
        potionNameText.text = potionData.itemName;

        if (iconPotion != null)
        {
            iconPotion.sprite = potionData.itemSprite;
        }
    }
}
