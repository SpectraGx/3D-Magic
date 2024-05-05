using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform itemAnchor;
    [SerializeField] private TileDetector tileDetector;

    private Tile closestTile;
    private Item item;

    private void Update()
    {
        // Obtiene el tile más cercano
        closestTile = tileDetector.GetClosestTile(transform.position);

        // Controla el brillo del tile más cercano
        if (closestTile)
        {
            var highlighter = closestTile.GetComponent<TileHighlighter>();
            if (highlighter)
            {
                highlighter.Highlight();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile))
        {
            tileDetector.AddTile(tile);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tile tile))
        {
            var highlighter = tile.GetComponent<TileHighlighter>();
            if (highlighter != null)
            {
                highlighter.RemoveHighlight();
            }

            tileDetector.RemoveTile(tile);
        }
    }

    public void PickUp(InputAction.CallbackContext callbackContext)
    {
        if (closestTile && callbackContext.performed)
        {
            closestTile.InteractPick(this, item);
        }
    }

    public void Interactuar(InputAction.CallbackContext callbackContext)
    {
        /*if (closestTile && callbackContext.performed)
        {
            closestTile.Interact(this,item);
        }*/

        if (closestTile != null)
        {
            if (callbackContext.performed)
            {
                closestTile.OnInteractStart(this);
            }
            if (callbackContext.canceled)
            {
                closestTile.OnInteractStop(this);
            }
        }
    }

    public bool GrabItem(Item _item)
    {
        if (item) return false;

        item = _item;
        item.transform.SetParent(itemAnchor, false);
        item.transform.localPosition = Vector3.zero;
        return true;
    }

    public bool HasIngredientObject()
    {
        return item != null;
    }

    public void DropItem()
    {
        item = null;
    }

}
