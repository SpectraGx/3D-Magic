using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBox : Tile
{
    public EventHandler OnPlayerGrabbedIngredient;
    [SerializeField] private BottleData bottleItem;

    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (player.HasIngredientObject()) return; // No instanciar si el jugador ya tiene un objeto

        // Instanciar el objeto del prefab en IngredientData
        Bottle newItem = Instantiate(bottleItem.prefab).GetComponent<Bottle>();

        if (player.GrabItem(newItem)) // Asignar el objeto al jugador
        {
            OnPlayerGrabbedIngredient?.Invoke(this, EventArgs.Empty); // Disparar evento si el objeto es tomado
        }
    }
}
