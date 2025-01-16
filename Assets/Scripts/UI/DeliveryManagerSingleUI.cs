using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetPotionData(IngredientData potionData){
        potionNameText.text = potionData.itemName;

        foreach(Transform child in iconContainer){
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(IngredientData ingredientData in potionData.ingredientsDataList){
            Transform iconTransform = Instantiate(iconTemplate,iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<UnityEngine.UI.Image>().sprite = ingredientData.itemSprite;
        }


    }
}
