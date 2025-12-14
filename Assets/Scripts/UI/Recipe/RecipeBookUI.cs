using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> recipePages;
    private int currentPageIndex = 0;

    private PlayerInput currentPlayerInput;

    private void Start()
    {
        /*
        foreach (var page in recipePages)
        {
            page.SetActive(false);
        }
        */
        CloseBookVisuals();
    }

    public void OpenBook(PlayerInput input)
    {
        currentPlayerInput = input;

        currentPlayerInput.SwitchCurrentActionMap("UI");


        currentPlayerInput.actions["Navigate"].performed += OnNavigate;
        currentPlayerInput.actions["Close"].performed += OnCloseBook;

        gameObject.SetActive(true);
        ShowPage(0);
    }

    public void CloseBook()
    {
        if (currentPlayerInput == null) return;

        currentPlayerInput.actions["Navigate"].performed -= OnNavigate;
        currentPlayerInput.actions["Close"].performed -= OnCloseBook;

        currentPlayerInput.SwitchCurrentActionMap("Player");
        currentPlayerInput = null;

        CloseBookVisuals();
    }

    private void OnCloseBook(InputAction.CallbackContext context)
    {
        CloseBook();
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (value.x > 0.5f) ShowNextPage();
        else if (value.x < -0.5) ShowPreviousPage();
    }

    private void CloseBookVisuals()
    {
        gameObject.SetActive(false);
        foreach(var page in recipePages) page.SetActive(false);
    }

    private void ShowPage (int index)
    {
        foreach(var page in recipePages) page.SetActive(false);

        if (recipePages.Count == 0) return;

        currentPageIndex = index;
        if (currentPageIndex >= recipePages.Count) currentPageIndex=0;
        if (currentPageIndex < 0) currentPageIndex = recipePages.Count -1;

        recipePages[currentPageIndex].SetActive(true);
    }

    public void ShowNextPage() => ShowPage(currentPageIndex +1);
    public void ShowPreviousPage() => ShowPage(currentPageIndex -1);
}
