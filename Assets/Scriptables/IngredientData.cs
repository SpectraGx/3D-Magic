using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType{
    Raw,
    Processed,
    Cooked
}
[CreateAssetMenu(menuName = "New Ingredient")]

public class IngredientData : ScriptableObject{
    public string itemName;
    public Sprite itemSprite;
    public IngredientType ingredientType;
    public GameObject prefab;
}