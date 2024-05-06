using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBox : Tile
{
    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (playerItem != null) {
            Destroy(playerItem.gameObject);
            player.DropItem();
            Debug.Log ("El objeto fue destruido");
        }
        else {
            Debug.Log ("No tienes un objeto para destruir");
        }
    }
}
