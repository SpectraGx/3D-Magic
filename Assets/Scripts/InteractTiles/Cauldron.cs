using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Tile
{
    /* public event EventHandler<OnIngredientAddedEventArgs> onIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public IngredientData ingredientData;
    } */

    [Header("Inspector")]
    [SerializeField] private List<IngredientData> validIngredientDataList;
    [SerializeField] private List<Transform> itemAnchors;
    private List<Item> items;

    public override void Awake()
    {
        base.Awake();
        items = new List<Item>(itemAnchors.Count); // Inicializa la lista de objetos
    }

    protected override bool GrabItem(Item newItem)
    {
        // Verificar si el objeto es un ingrediente válido
        IngredientData ingredientData = newItem.GetComponent<Ingredient>().GetIngredientData();
        if (ingredientData == null || !validIngredientDataList.Contains(ingredientData))
        {
            Debug.Log("El objeto no es un ingrediente válido para el caldero.");
            return false; // El objeto no es válido
        }

        // Encuentra el primer punto de anclaje disponible
        for (int i = 0; i < itemAnchors.Count; i++)
        {
            if (items.Count <= i || items[i] == null) // Si el anclaje está vacío
            {
                newItem.transform.SetParent(itemAnchors[i], false); // Establece el padre
                newItem.transform.localPosition = Vector3.zero;

                if (items.Count <= i)
                {
                    items.Add(newItem); // Añadir a la lista si es nuevo
                }
                else
                {
                    items[i] = newItem; // Reemplazar el objeto en esa posición
                }

                return true; // Indicar que se agregó con éxito
            }
        }

        return false; // No hay espacio disponible
    }

    public override void InteractPick(PlayerInteraction owner, Item playerItem)
    {
        if (playerItem != null) // Si el jugador tiene un objeto
        {
            // Intentar agregar el objeto al caldero
            if (GrabItem(playerItem))
            {
                owner.DropItem(); // El jugador suelta el objeto
                Debug.Log("Ingrediente agregado al caldero");
            }
            else
            {
                Debug.Log("No se puede agregar el objeto al caldero.");
            }
        }
        else
        {
            Debug.Log("No hay objeto para agregar.");
        }
    }
}
