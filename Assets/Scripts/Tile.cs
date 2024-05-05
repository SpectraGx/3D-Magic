using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] private Transform itemAnchor;
    [SerializeField] protected Item initialItem;

    protected Item item;

    protected virtual void Awake()
    {
        if (initialItem != null)
        {
            GrabItem(initialItem);
        }
    }

    public virtual void Interact(PlayerInteraction player)
    {
        Debug.Log("Interact");
    }

    public virtual void TakeAction(PlayerInteraction owner, Item playerItem)
    {
        if (item == null && playerItem != null)
        {
            if (GrabItem(playerItem))
            {
                owner.DropItem();
            }
        }
        else if (item != null && playerItem == null)
        {
            if (owner.GrabItem(item))
            {
                DropItem();
            }
        }
    }

    protected virtual bool GrabItem(Item _item)
    {
        if (item != null) return false;

        item = _item;
        item.transform.SetParent(itemAnchor, false);
        item.transform.localPosition = Vector3.zero;

        return true;
    }

    protected virtual void DropItem()
    {
        item = null;
    }
}
