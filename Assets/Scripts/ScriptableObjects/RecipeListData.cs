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
        if (recipe.ingredientDatas.Count != ingredientDatas.Count)
        {
            return false; // Si las listas no tienen la misma cantidad de ingredientes, no coinciden
        }

        // Crear una copia de la lista para evitar modificar la original
        var tempIngredientDatas = new List<IngredientData>(ingredientDatas);

        // Verificar que cada ingrediente de la receta esté presente en `tempIngredientDatas`
        foreach (var recipeIngredient in recipe.ingredientDatas)
        {
            if (tempIngredientDatas.Contains(recipeIngredient))
            {
                tempIngredientDatas.Remove(recipeIngredient); // Remover para evitar duplicados
            }
            else
            {
                return false; // Si falta un ingrediente, no coincide
            }
        }

        // Si se removieron todos los ingredientes requeridos y no queda ninguno adicional
        return tempIngredientDatas.Count == 0;
    }
}
