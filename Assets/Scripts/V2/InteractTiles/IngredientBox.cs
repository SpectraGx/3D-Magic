using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientBox : Tile
{
    public EventHandler OnPlayerGrabbedIngredient;
    [SerializeField] private IngredientData ingredientItem;

    public override void TakeAction(PlayerInteraction player, Item playerItem)
    {
        if (player.HasIngredientObject()) return; // No instanciar si el jugador ya tiene un objeto

        // Instanciar el objeto del prefab en IngredientData
        Ingredient newItem = Instantiate(ingredientItem.prefab).GetComponent<Ingredient>();

        if (player.GrabItem(newItem)) // Asignar el objeto al jugador
        {
            OnPlayerGrabbedIngredient?.Invoke(this, EventArgs.Empty); // Disparar evento si el objeto es tomado
        }
    }
}
