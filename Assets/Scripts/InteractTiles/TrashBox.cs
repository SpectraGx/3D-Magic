using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBox : Tile
{
    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (playerItem != null) {                   // Si el jugador tiene un objeto
            Destroy(playerItem.gameObject);         // Destruir el objeto que tiene el jugador
            player.DropItem();                      // Dropea el objeto
            Debug.Log ("El objeto fue destruido");
        }
        else {
            Debug.Log ("No tienes un objeto para destruir");
        }
    }
}
