using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecipeBookTile : Tile
{
    [SerializeField] private RecipeBookUI recipeBookUI;

    public override void OnInteractStart(PlayerInteraction player)
    {
        if (recipeBookUI != null)
        {
            recipeBookUI.ToggleBook();
            //playerInput.SwitchCurrentActionMap("UI");
        }
    }
}
