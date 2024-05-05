/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBox : Tile
{
    [Header("Inspector")]
    [SerializeField] private IngredientData ingredientItem;

    protected override void Awake()
    {
        base.Awake();
    }

    //          METODO PUBLICO
    public override void ActionComplete()
    {

    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        owner.GrabItem(Instantiate(ingredientItem.prefab).GetComponent<Item>());
    }
}
*/
