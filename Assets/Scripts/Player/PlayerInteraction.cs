using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private Transform itemPick;

    [Header("Variables privadas")]
    private List<Tile> _tileCloseList = new List<Tile>();
    private Tile _closestTile;
    private Item _item;

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void DropItem()
    {

    }

    public void RemoveItem()
    {

    }

}
