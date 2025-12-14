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
            PlayerInput input = player.GetComponent<PlayerInput>();
            if (input != null)
            {
                recipeBookUI.OpenBook(input);

                //recipeBookUI.ToggleBook();

                //playerInput.SwitchCurrentActionMap("UI");
            }
        }
    }

    public override void OnInteractStop(PlayerInteraction player)
    {
        base.OnInteractStop(player);
    }
}
