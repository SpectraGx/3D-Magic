using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class CuttingTable : Tile
{/*
  
    [Header("Inspector")]
    [SerializeField] private float timeToCut = 3;

    [Header("Variables privadas")]
    private float _timeToCut;
    private bool _isCutting;

    private void Start()
    {
        _timeToCut = timeToCut;
    }

    private void Update()
    {
        if (!_isCutting) return;

        _timeToCut -= Time.deltaTime;

        if (_timeToCut <= 0 && item.TryGetComponent(out Ingredient ingredient) && ingredient.CanBeCut())
        {
            if (CraftFood(ingredient.GetNextIngredientData()))
            {
                ActionComplete();
                _timeToCut = timeToCut;
            }
        }
    }

    //      METODOS PUBLICOS
    public override void TakeAction(PlayerInteraction owner, Item playerItem, Action _onActionComplete)
    {
        onActionComplete = _onActionComplete;

        if (playerItem && !item)
        {
            if (GrabItem(playerItem)) owner.DropItem();
        }
        else if (!playerItem && item)
        {
            if (item.TryGetComponent(out Ingredient ingredient) && ingredient.CanBeCut())
            {
                TakeAdvanceAction(owner);
            }
            else
            {
                if (owner.GrabItem(item)) DropItem();
            }
        }
    }

    public override void ActionComplete()
    {
        if (!item && !_isCutting) return;

        _isCutting = false;
        onActionComplete();
    }

    //      METODOS PRIVADOS
    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        _timeToCut = timeToCut;
        _isCutting = true;
    }

    //      Remplaza el objeto actual por uno nuevo
    
    */

    public override void InteractPick(PlayerInteraction owner, Item playerItem)
    {
        base.InteractPick(owner, playerItem);
    }

    public override void Interact(PlayerInteraction player, Item playerItem)
    {
        if (item != null && item.TryGetComponent(out Ingredient ingredient))
        {
            // Verifica si el ingrediente puede ser cortado
            if (ingredient.CanBeCut())
            {
                // Reemplaza el objeto actual con uno nuevo usando CraftFood
                if (CraftFood(ingredient.GetNextIngredientData()))
                {
                    Debug.Log("El ingrediente fue remplazado por uno cortado");
                }
            }
            else
            {
                Debug.Log("El ingrediente no puede ser cortado");
            }
        }
        else
        {
            Debug.Log("La Cutting Table no tiene objeto");
        }
    }

    private bool CraftFood(IngredientData newIngredientData)
    {
        if (item == null) return false; // No reemplazar si no hay objeto

        // Instancia el nuevo objeto usando el prefab de IngredientData
        GameObject newFoodClone = Instantiate(newIngredientData.prefab, item.transform.parent, false);

        // Destruye el objeto anterior
        Destroy(item.gameObject);

        // Asigna el nuevo objeto como el actual
        item = newFoodClone.GetComponent<Item>();

        return true; // Devuelve true para indicar que se reemplazó con éxito
    }

}
 