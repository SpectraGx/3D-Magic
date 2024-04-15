using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Bonfire : Tile
{
    /*
    // Public Methods *****
    public override void ActionComplete()
    {
        //throw new System.NotImplementedException();
    }

    // Private Methods *****
    protected override bool GrabItem(Item _item)
    {
        bool canGrabItem = base.GrabItem(item);
        if (canGrabItem && _item.TryGetComponent(out Cauldron cookware))
        {
            cookware.OnFullAndValidRecipe += Cookware_OnFullAndValidRecipe;
            if(cookware.CanBeCooked()) StartCook(cookware);
        }

        return canGrabItem;
    }

    protected override void DropItem()
    {
        if (item.TryGetComponent(out Cauldron cookware))
        {
            cookware.OnFullAndValidRecipe -= Cookware_OnFullAndValidRecipe;
            cookware.StopCook();
        }
        base.DropItem();
    }

    private void Cookware_OnFullAndValidRecipe(object sender, Cauldron cookware)
    {
        StartCook(cookware);
    }

    private void StartCook(Cookware cookware)
    {
        Debug.Log("Stove try to start cook");
        cookware.StartCook();
    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        
    }
    
}
*/
