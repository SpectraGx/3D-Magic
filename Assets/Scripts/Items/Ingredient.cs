using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    
    [Header("Inspector: Scriptables")]
    [SerializeField] private IngredientData ingredientData;
    [SerializeField] private IngredientData nextIngredientData;
    
    [Header("Variable Privada")]
    private MeshFilter _mesh;

    // Metodo Publico

    public bool CanBeCut() => ingredientData.ingredientType == IngredientType.Raw;
    public IngredientData GetIngredientData() => ingredientData;
    public IngredientData GetNextIngredientData() => nextIngredientData;

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public static Ingredient SpawnIngredientObject(IngredientData ingredientData, Transform parent){
        GameObject newPotion = Instantiate(ingredientData.prefab, parent);
        Ingredient ingredient = newPotion.GetComponent<Ingredient>();
        return ingredient;
    }
    
}
