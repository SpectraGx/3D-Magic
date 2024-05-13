using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : Tile
{
    [SerializeField] private ParticleSystem starsParticles;
    public override void Awake()
    {
        base.Awake();
        starsParticles.Stop();
    }
    protected bool IsValidIngredient(Item item)
    {
        // Verificar si el item es un ingrediente
        var ingredient = item.GetComponent<Ingredient>();
        if (ingredient == null) return false;

        // Obtener los datos de la pocion
        var ingredientData = ingredient.GetIngredientData();
        if (ingredientData == null) return false;

        // Verificar si el tipo de ingrediente es "cooked"
        return ingredientData.ingredientType == IngredientType.Cooked;
    }

    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (item == null && playerItem != null)
        {
            // Verificar si el ingrediente es válido para colocar en la mesa
            if (IsValidIngredient(playerItem))
            {
                if (GrabItem(playerItem))
                {
                    player.DropItem();
                    var ingredient = item.GetComponent<Ingredient>();
                    DeliveryManager.Instance.DeliveryPotion(ingredient);
                    ingredient.DestroySelf();
                    item = null;
                }
            }
            else
            {
                Debug.Log("Solo se pueden colocar pociones");
            }
        }
    }

    public void ActivateDeliveryParticles()
    {
        if (starsParticles != null)
        {
            starsParticles.Play();
        }
    }

    public void DesactivateDeliveryParticles()
    {
        if (starsParticles != null)
        {
            starsParticles.Stop();
        }
    }
}
