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
        int check3 = 0;
        // Comprueba si todos los ingredientes de la receta están en la lista proporcionada
        //return recipe.ingredientDatas.All(ingredientDatas.Contains);
        for (int i = 0; i < ingredientDatas.Count; i++) {
            if (recipe.ingredientDatas.Contains(ingredientDatas[i])){
                check3++;
            }
        }
        if (check3 == 3) { return true; } else { return false; }
    }
}
