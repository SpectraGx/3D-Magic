using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] private Transform itemAnchor;
    private List<Tile> tileCloseList = new List<Tile>();
    private Tile closestTile;
    private Item item;

    private void Update()
    {
        if (!closestTile) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && !tileCloseList.Contains(tile))
        {
            tileCloseList.Add(tile);

            if (closestTile) closestTile.StopHighlight();
            closestTile = GetCloserTile();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tile tile))
        {
            tileCloseList.Remove(tile);
            if (closestTile) closestTile.StopHighlight();
            closestTile = GetCloserTile();
        }
    }

    //          METODO PRIVADO

    private Tile GetCloserTile()
    {
        if (tileCloseList.Count <= 0) return null;
        Tile winnerTile = null;
        float minDistance = Mathf.Infinity;
        foreach (Tile tile in tileCloseList)
        {
            float newDistance = Vector3.Distance(transform.position, tile.transform.position);
            if (newDistance <= minDistance)
            {
                winnerTile = tile;
                minDistance = newDistance;
            }
        }

        if (winnerTile) winnerTile.StartHighlight();
        return winnerTile;
    }


    //          METODOS PUBLICOS

    public bool GrabItem(Item _item)
    {
        if (item) return false;
        item = _item;
        item.transform.SetParent(itemAnchor, false);
        item.transform.localPosition = Vector3.zero;
        return true;
    }

    public void DropItem()
    {
        item = null;
    }

    public void RemoveItem()
    {
        Destroy(item.gameObject);
        DropItem();
    }

    public void PickUp(InputAction.CallbackContext callbackContext)
    {
        if (closestTile && callbackContext.performed)
        {
            closestTile.TakeAction(this, item, RecogerAnim);
            Debug.Log("Recoger");
        }
         /*if (callbackContext.canceled)
        {
            closestTile.ActionComplete();
        }*/
    }

    public void Interactuar (InputAction.CallbackContext callbackContext){
        if (closestTile && callbackContext.performed){
            closestTile.TakeAction(this,null,Cut);
            Debug.Log("Interactuar");
        }
    }

    public void Cut(){
        Debug.Log("Cortar");
    }
    
    public void RecogerAnim()
    {

    }
}
