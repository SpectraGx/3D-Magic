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

    protected virtual void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
        if (meshRenderer)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                Material[] materials;
                (materials = meshRenderer.materials)[i].EnableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                meshRenderer.materials[i].SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
        }
        else if (skinnedMeshRenderer)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                Material[] materials;
                (materials = skinnedMeshRenderer.materials)[i].EnableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                skinnedMeshRenderer.materials[i].SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
        }
    }

    public void StopHighlight()
    {
        if (meshRenderer)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                //meshRenderer.materials[i].color = _baseColors[i];
                Material[] materials;
                (materials = meshRenderer.materials)[i].DisableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                meshRenderer.materials[i].SetColor("_EmissionColor", Color.black);
            }
        }
        else if (skinnedMeshRenderer)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                //meshRenderer.materials[i].color = _baseColors[i];
                Material[] materials;
                (materials = skinnedMeshRenderer.materials)[i].DisableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                skinnedMeshRenderer.materials[i].SetColor("_EmissionColor", Color.black);
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
