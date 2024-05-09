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
    public EventHandler OnPlayerGrabbedIngredient;


    [Header("Inspector")]
    [SerializeField] private List<IngredientData> validIngredientDataList;
    [SerializeField] private RecipeListData recipeList; // Lista de todas las recetas
    [SerializeField] private List<Transform> itemAnchors;
    //[SerializeField] private float cookingTime; // Tiempo máximo para cocinar

    private List<Item> items;
    private State state;
    private float cookingTimer;
    [SerializeField] private RecipeData activeRecipe; // La receta activa
    [SerializeField] private GameObject currentLiquid;
    private PlayerInteraction pItemAnchor;

    private List<IngredientData> calderoIngredients = new List<IngredientData>();
    


    public override void Awake()
    {
        base.Awake();
        items = new List<Item>(itemAnchors.Count); // Inicializa la lista de objetos
        state = State.Idle;
        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0 // Iniciar el progreso en 0
        });
    }

    public override void InteractPick(PlayerInteraction player, Item playerItem)
    {
        if (state == State.Idle) // Solo permite agregar ingredientes cuando está en estado Idle
        {
            if (playerItem != null) // Si el jugador tiene un objeto
            {
                if (GrabItem(playerItem)) // Intentar agregar el objeto al caldero
                {
                    player.DropItem(); // El jugador suelta el objeto
                    Debug.Log("Ingrediente agregado al caldero");

                    if (items.Count == itemAnchors.Count) // Si el caldero está lleno
                    {
                        StartCooking(); // Iniciar el proceso de cocción
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

                    Transform playerItemAnchor = player.GetItemAnchor(); // Método para obtener el ItemAnchor del jugador

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
        state = State.Cooking; // Cambia al estado Cooking
        cookingTimer = 0f; // Restablece el temporizador

        /* //var calderoIngredients = new List<IngredientData>();
        foreach (var item in items)
        {
            var ingredientData = item.GetComponent<Ingredient>().GetIngredientData();
            if (ingredientData != null)
            {
                calderoIngredients.Add(ingredientData);
            }
        } */
        IngredientAdded();

        activeRecipe = recipeList.FindMatchingRecipe(calderoIngredients); // Encuentra la receta correcta
        if (activeRecipe == null)
        {
            Debug.LogError("No se encontró receta para los ingredientes actuales.");
            state = State.Idle;
            return;
        }

        Debug.Log("El caldero está cocinando...");
    }

    public void IngredientAdded(){
        //var calderoIngredients = new List<IngredientData>();
        foreach (var item in items)
        {
            var ingredientData = item.GetComponent<Ingredient>().GetIngredientData();
            if (ingredientData != null)
            {
                calderoIngredients.Add(ingredientData);
            }
        }
    }

    private void Update()
    {
        if (state == State.Cooking)
        {
            cookingTimer += Time.deltaTime;

            OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
            {
                progressNormalized = cookingTimer / activeRecipe.cookingTime // Tiempo máximo de cocción
            });

            if (cookingTimer >= activeRecipe.cookingTime)
            {
                CompleteCooking();
            }
        }
    }

    private void CompleteCooking()
    {
        // Destruye todos los ingredientes anteriores
        foreach (var item in items)
        {
            item?.GetComponent<Ingredient>().DestroySelf();
        }

        items.Clear(); // Limpia la lista de ingredientes
        //currentLiquid = Ingredient.SpawnIngredientObject(activeRecipe.result, itemAnchors[0]); // Instanciar el resultado final
        currentLiquid = Instantiate(activeRecipe.result.prefab, itemAnchors[0]);

        state = State.Completed; // Cambia al estado Completed

        Debug.Log("El caldero ha completado la cocción y ha creado una poción.");

        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0
        });
    }

    protected override bool GrabItem(Item newItem)
    {
        // Verificar si el objeto es un ingrediente válido
        if (newItem == null || !IsValidIngredient(newItem))
        {
            Debug.Log("El objeto no es un ingrediente válido para el caldero.");
            return false;
        }

        // Encuentra el primer itemAnchor disponible
        for (int i = 0; i < itemAnchors.Count; i++)
        {
            if (items.Count <= i || items[i] == null) // Si el itemAnchor está vacío
            {
                newItem.transform.SetParent(itemAnchors[i], false);
                newItem.transform.localPosition = Vector3.zero;

                if (items.Count <= i)
                {
                    items.Add(newItem);
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

    private bool CraftFood(IngredientData newIngredientData)
    {
        if (item == null) return false; // No reemplazar si no hay objeto

        // Instancia usando el prefab de IngredientData
        GameObject newFoodClone = Instantiate(newIngredientData.prefab, item.transform.parent, false);

        Destroy(item.gameObject);

        // Asigna el nuevo objeto como el actual
        item = newFoodClone.GetComponent<Item>();

        return true; // Devuelve true para indicar que se reemplazó con éxito
    }
}
