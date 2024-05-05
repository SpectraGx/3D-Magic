using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void Highlight()
    {
        ApplyEmission(new Color(0.2f, 0.2f, 0.2f));
    }

    public void RemoveHighlight()
    {
        ApplyEmission(Color.black);
    }

    private void ApplyEmission(Color emissionColor)
    {
        foreach (var renderer in meshRenderers)
        {
            foreach (var material in renderer.materials)
            {
                if (emissionColor == Color.black)
                {
                    material.DisableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                }
                else
                {
                    material.EnableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                }
                material.SetColor("_EmissionColor", emissionColor);
            }
        }

        if (skinnedMeshRenderer != null)
        {
            foreach (var material in skinnedMeshRenderer.materials)
            {
                if (emissionColor == Color.black)
                {
                    material.DisableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                }
                else
                {
                    material.EnableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                }
                material.SetColor("_EmissionColor", emissionColor);
            }
        }
    }
}