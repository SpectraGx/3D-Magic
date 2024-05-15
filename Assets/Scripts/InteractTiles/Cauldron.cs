using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Tile, UIProgress
{
    public enum State
    {
        Idle,
        Cooking,
        Completed
    }

    public event EventHandler<UIProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded; // Nuevo evento para indicar que se agrego un ingrediente al caldero
    public class OnIngredientAddedEventArgs : EventArgs{
        public Item ingredient1;
    }

    [Header("Inspector")]
    [SerializeField] private List<IngredientData> validIngredientDataList;
    [SerializeField] private RecipeListData recipeList; // Lista de todas las recetas
    [SerializeField] private List<Transform> itemAnchors;

    private List<Item> items;
    private State state;
    private float cookingTimer;
    [SerializeField] private RecipeData activeRecipe; // La receta activa
    [SerializeField] private GameObject currentLiquid;
    [SerializeField] private IconUI iconUI;

    [SerializeField] private ParticleSystem smokeParticle;

    public override void Awake()
    {
        base.Awake();
        items = new List<Item>(itemAnchors.Count);
        state = State.Idle;
        smokeParticle.Stop();
        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0
        });
    }

    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (state == State.Idle)
        {
            if (playerItem != null)
            {
                if (GrabItem(playerItem))
                {
                    player.DropItem();
                    Debug.Log("Ingrediente agregado al caldero");
                    if (items.Count == itemAnchors.Count)
                    {
                        StartCooking();
                    }
                }
                else
                {
                    Debug.Log("No se puede agregar al caldero.");
                }
            }
            else
            {
                Debug.Log("No hay objeto para agregar.");
            }
        }
        else if (state == State.Completed)
        {
            if (playerItem != null && playerItem.TryGetComponent(out Ingredient ingredient) && ingredient.CanBeFill())
            {
                Debug.Log("El jugador tiene una botella vacía");

                // Instanciar el nuevo objeto usando el siguiente ingrediente del liquido
                currentLiquid.TryGetComponent(out Ingredient ingredientLiquid);
                var newLiquidData = ingredientLiquid.GetNextIngredientData();
                if (newLiquidData != null)
                {
                    Destroy(playerItem.gameObject);

                    Transform playerItemAnchor = player.GetItemAnchor(); // Metodo para obtener el ItemAnchor del jugador

                    // Instanciar el nuevo objeto como hijo del ItemAnchor del jugador
                    Ingredient newIngredient = Ingredient.SpawnIngredientObject(newLiquidData, playerItemAnchor);
                    //item = newIngredient.GetComponent<Item>();

                    // Eliminar el objeto botello

                    // Asignar el nuevo objeto al jugador
                    item = newIngredient.GetComponent<Item>();
                    player.item = null;
                    player.GrabItem(newIngredient);
                    
                    /* if (player.GrabItem(newIngredient)) // Asignar el objeto al jugador
                    {
                        OnPlayerGrabbedIngredient?.Invoke(this, EventArgs.Empty); // Disparar evento si el objeto es tomado
                    } */


                    Destroy(currentLiquid);

                    Debug.Log("La botella fue reemplazada por la nueva poción");
                    //OnIngredientAdded = null;
                    iconUI.ResetVisual();
                    state = State.Idle;
                }
            }
        }
        else
        {
            Debug.Log("El caldero está cocinando.");
        }
    }

    private void StartCooking()
    {
        state = State.Cooking;
        cookingTimer = 0f;
        activeRecipe = recipeList.FindMatchingRecipe(GetCauldronIngredientDataList()); // Utiliza el nuevo metodo para obtener los ingredientes
        if (activeRecipe == null)
        {
            Debug.LogError("No se encontró receta para los ingredientes actuales.");
            state = State.Idle;
            return;
        }

        Debug.Log("El caldero está cocinando...");
        smokeParticle.Play();

    }

    public List<IngredientData> GetCauldronIngredientDataList()
    {
        List<IngredientData> ingredientDataList = new List<IngredientData>();
        foreach (var item in items)
        {
            var ingredientData = item.GetComponent<Ingredient>().GetIngredientData();
            if (ingredientData != null)
            {
                ingredientDataList.Add(ingredientData);
            }
        }
        return ingredientDataList;
    }

    private void Update()
    {
        if (state == State.Cooking)
        {
            cookingTimer += Time.deltaTime;
            OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
            {
                progressNormalized = cookingTimer / activeRecipe.cookingTime
            });

            if (cookingTimer >= activeRecipe.cookingTime)
            {
                CompleteCooking();
            }
        }
    }

    private void CompleteCooking()
    {
        foreach (var item in items)
        {
            item?.GetComponent<Ingredient>().DestroySelf();
        }

        items.Clear();
        currentLiquid = Instantiate(activeRecipe.result.prefab, itemAnchors[0]);
        state = State.Completed;

        Debug.Log("El caldero ha completado la cocción y ha creado una poción.");
        smokeParticle.Stop();

        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0
        });
    }

    protected override bool GrabItem(Item newItem)
    {
        if (newItem == null || !IsValidIngredient(newItem))
        {
            Debug.Log("El objeto no es un ingrediente válido para el caldero.");
            return false;
        }

        for (int i = 0; i < itemAnchors.Count; i++)
        {
            if (items.Count <= i || items[i] == null)
            {
                newItem.transform.SetParent(itemAnchors[i], false);
                newItem.transform.localPosition = Vector3.zero;

                if (items.Count <= i)
                {
                    items.Add(newItem);
                    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{
                        ingredient1 = newItem
                    }); // Dispara el evento de ingrediente agregado
                }
                else
                {
                    items[i] = newItem;
                }
                return true;
            }
        }
        return false;
    }

    private bool IsValidIngredient(Item item)
    {
        var ingredient = item.GetComponent<Ingredient>();
        if (ingredient == null) return false;

        var ingredientData = ingredient.GetIngredientData();
        return ingredientData != null && validIngredientDataList.Contains(ingredientData);
    }
}
