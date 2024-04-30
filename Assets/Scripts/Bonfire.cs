using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Tile
{
    
    //          METODO PUBLICO
    public override void ActionComplete()
    {
        //throw new System.NotImplementedException();
    }

    //          METODOS PRIVADOS
    protected override bool GrabItem(Item _item)
    {
        bool canGrabItem = base.GrabItem(_item);
        if (canGrabItem && item.TryGetComponent(out Cauldron cauldron))
        {
            cauldron.OnFullAndValidRecipe += Cookware_OnFullAndValidRecipe;
            if(cauldron.CanBeCooked()) StartCook(cauldron);
        }

        return canGrabItem;
    }

    protected override void DropItem()
    {
        if (item.TryGetComponent(out Cauldron cauldron))
        {
            cauldron.OnFullAndValidRecipe -= Cookware_OnFullAndValidRecipe;
            cauldron.StopCook();
        }
        base.DropItem();
    }

    private void Cookware_OnFullAndValidRecipe(object sender, Cauldron cauldron)
    {
        StartCook(cauldron);
    }

    private void StartCook(Cauldron cauldron)
    {
        Debug.Log("El caldero intenta preparar");
        cauldron.StartCook();
    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        
    }
    
}

