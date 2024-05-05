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

    [Header("Inspector")]
    [SerializeField] private float timeToCut = 3f;
    [Header("Variables privadas")]
    private float cutTimer = 0f;
    private bool _isCutting = false;


    public override void OnInteractStart(PlayerInteraction player)
    {
        if (item != null && item.TryGetComponent(out Ingredient ingredient) && ingredient.CanBeCut())
        {
            _isCutting = true;  // Empieza a cortar
        }
    }

    public override void OnInteractStop(PlayerInteraction player)
    {
        _isCutting = false;     // Deja de cortar
        cutTimer = 0;
    }

    private void Update()
    {
        if (_isCutting) // Si esta cortando
        {
            cutTimer += Time.deltaTime; // Timer de cortar
            if (cutTimer >= timeToCut)  // Si se mantuvo cortando la cantidad de tiempo necesitada
            {
                // Remplazar por objeto cortado
                if (item.TryGetComponent(out Ingredient ingredient) && CraftFood(ingredient.GetNextIngredientData()))
                {
                    Debug.Log("El ingrediente fue remplazado por uno cortado");
                    _isCutting = false; // Reseteo de corte
                    cutTimer = 0f;      // Reincio de timer
                }
            }
        }
    }


    public override void InteractPick(PlayerInteraction owner, Item playerItem)
    {
        base.InteractPick(owner, playerItem);
    }

    /*
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
    */

    private bool CraftFood(IngredientData newIngredientData)
    {
        if (item == null) return false; // No reemplazar si no hay objeto

        // Instancia usando el prefab de IngredientData
        GameObject newFoodClone = Instantiate(newIngredientData.prefab, item.transform.parent, false);

        Destroy(item.gameObject);

        // Asigna el nuevo objeto como el actual
        item = newFoodClone.GetComponent<Item>();

        return true; // Devuelve true para indicar que se reemplazó con éxito
    }

}
