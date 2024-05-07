using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "New Recipe List")]
public class RecipeListData : ScriptableObject
{
    public List<RecipeData> recipeListData;

    public RecipeData FindMatchingRecipe(List<IngredientData> ingredientDatas)
    {
        foreach (var recipe in recipeListData)
        {
            if (RecipeMatches(recipe, ingredientDatas))
            {
                return recipe; // Devuelve la receta correspondiente
            }
            //Debug.Log($"El ingrediente puesto es {ingredientDatas.itemName} segundos");
        }
        return null; // No se encontró ninguna receta que coincida
    }

    private bool RecipeMatches(RecipeData recipe, List<IngredientData> ingredientDatas)
    {
        // Comprueba si todos los ingredientes de la receta están en la lista proporcionada
        return recipe.ingredientDatas.All(ingredientDatas.Contains);
    }
}
