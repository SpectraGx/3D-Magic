using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] protected Transform itemAnchor;
    [Header("Referencia")]
    [SerializeField] protected Item initialItem;
    [Header("Variables Privadas")]
    protected Item item;
    protected Action onActionComplete;

    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();


    protected virtual void Awake()
    {
        meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        if (initialItem)
        {
            GrabItem(initialItem);
        }
    }

    //          METODOS PUBLICOS
    //          CAMBIAR ESTO PARA CORTAR

    public virtual void TakeAction(PlayerInteraction owner, Item playerItem, Action _onActionComplete)
    {
        onActionComplete = _onActionComplete;
        if (!item && playerItem)
        {
            if (GrabItem(playerItem)) owner.DropItem();
        }
        else if (item && !playerItem)
        {
            if (owner.GrabItem(item)) DropItem();
        }
        else
        {
            TakeAdvanceAction(owner);
        }
    }

    public void StartHighlight()
    {
        foreach (var renderer in meshRenderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                material.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
        }

        if (skinnedMeshRenderer != null)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                var material = skinnedMeshRenderer.materials[i];
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                material.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
        }
    }

    public void StopHighlight()
    {
        foreach (var renderer in meshRenderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                material.DisableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                material.SetColor("_EmissionColor", Color.black);
            }
        }

        if (skinnedMeshRenderer != null)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                var material = skinnedMeshRenderer.materials[i];
                material.DisableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public abstract void ActionComplete();

    //          METODOS PRIVADOS    

    protected abstract void TakeAdvanceAction(PlayerInteraction owner);

    protected virtual bool GrabItem(Item _item)
    {
        if (item) return false;

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
