using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> recipePages;
    private int currentPageIndex = 0;

    private void Start()
    {
        foreach (var page in recipePages)
        {
            page.SetActive(false);
        }
    }

    public void ShowNextPage()
    {
        if (recipePages.Count == 0) return;
        recipePages[currentPageIndex].SetActive(false);
        currentPageIndex = (currentPageIndex + 1) % recipePages.Count;
        recipePages[currentPageIndex].SetActive(true);
    }

    public void ShowPreviousPage()
    {
        if (recipePages.Count == 0) return;
        recipePages[currentPageIndex].SetActive(false);
        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = recipePages.Count - 1;
        }
        recipePages[currentPageIndex].SetActive(true);
    }

    public void ToggleBook()
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);

        if (isActive)
        {
            currentPageIndex = 0;
            foreach (var page in recipePages)
            {
                page.SetActive(false);
            }
            if (recipePages.Count > 0)
            {
                recipePages[currentPageIndex].SetActive(true);
            }
        }
    }
}
