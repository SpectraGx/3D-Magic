using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBox : Tile
{
    public EventHandler OnPlayerGrabbedIngredient;

    [Header("Bottle Data")]
    [SerializeField] private List<IngredientData> possibleBottles; 

    // Método para obtener una botella aleatoria
    private IngredientData GetRandomIngredientData()
    {
        if (possibleBottles == null || possibleBottles.Count == 0)
        {
            Debug.LogError("No hay botellas en la lista");
            return null; // No instanciar si no hay objetos en la lista
        }

        int randomIndex = UnityEngine.Random.Range(0, possibleBottles.Count); // Elegir un índice aleatorio
        return possibleBottles[randomIndex]; // Devolver la botella aleatoria
    }

    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (player.HasIngredientObject()) return; // No instanciar si el jugador ya tiene un objeto

        // Obtener una botella aleatoria
        IngredientData randomIngredientData = GetRandomIngredientData();

        if (randomIngredientData == null) return; // No instanciar si el dato es nulo

        // Instanciar el objeto del prefab de la botella aleatoria
        Ingredient newItem = Instantiate(randomIngredientData.prefab).GetComponent<Ingredient>();

        if (player.GrabItem(newItem)) // Asignar el objeto al jugador
        {
            OnPlayerGrabbedIngredient?.Invoke(this, EventArgs.Empty); // Disparar evento si el objeto es tomado
        }
    }
}
