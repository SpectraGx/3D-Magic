using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Item
{
    [Header("Inspector: Scriptables")]
    [SerializeField] private BottleData bottleData;
    [SerializeField] private BottleData bottleFill;
    
    [Header("Variable Privada")]
    private MeshFilter _mesh;

    // Metodo Publico

    public BottleData GetBottleData() => bottleData;
    public BottleData GetNextBottleData() => bottleFill;

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public static Bottle SpawnIngredientObject(BottleData ingredientData, Transform parent){
        GameObject newPotion = Instantiate(ingredientData.prefab, parent);
        Bottle bottle = newPotion.GetComponent<Bottle>();
        return bottle;
    }
}
