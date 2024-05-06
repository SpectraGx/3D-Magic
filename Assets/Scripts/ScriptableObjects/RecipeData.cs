using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Recipe")]
public class RecipeData : ScriptableObject
{
   public List <IngredientData> ingredientDatas;
   public string recipeName;
}
