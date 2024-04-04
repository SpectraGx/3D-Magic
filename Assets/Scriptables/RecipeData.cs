using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct IngredientAmount {
    public IngredientData ingredient;
    [Range (1,999)]
    public int amount;
}

[CreateAssetMenu (menuName = "New Recipe")]
public class RecipeData : ScriptableObject{
    public List<IngredientAmount> materials;
    public IngredientData result;
    [Range(0,999)]
    public float cookingTime;
}