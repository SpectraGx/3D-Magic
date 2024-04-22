using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class CuttingTable : Tile
{
  
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
    private bool CraftFood(IngredientData newIngredientData)
    {
        if (!item) return false;

        GameObject newFoodClone = Instantiate(newIngredientData.prefab, item.transform.parent, false);
        Destroy(item.gameObject);
        item = newFoodClone.GetComponent<Item>();
        return true;
    }
   
}
 