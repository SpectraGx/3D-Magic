using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetIngredientObjectData (IngredientData ingredientData){
        image.sprite = ingredientData.itemSprite;
    }
}
