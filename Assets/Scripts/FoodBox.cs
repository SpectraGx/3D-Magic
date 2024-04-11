using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBox : Tile
{
    [Header("Inspector")]
    [SerializeField] private IngredientData ingredientItem;
    [Header("Variable Privado")]
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        //animator = GetComponent<Animator>();
    }

    //          METODO PUBLICO
    public override void ActionComplete()
    {

    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        //animator.SetTrigger("Open");
        owner.GrabItem(Instantiate(ingredientItem.prefab).GetComponent<Item>());
    }
}
