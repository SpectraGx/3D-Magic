using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Tile, UIProgress
{
    /* public enum State
    {
        Idle,
        Cooking,
        Completed
    }

    public event EventHandler<UIProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [Header("Inspector")]
    [SerializeField] private List<IngredientData> validIngredientDataList;
    [SerializeField] private List<Transform> itemAnchors;
    [SerializeField] private IngredientData potionResult; // Objeto final (Poción)
    [SerializeField] private float cookingTime; // Tiempo máximo para cocinar

    private List<Item> items;
    private State state;
    private float cookingTimer;

    public override void Awake()
    {
        base.Awake();
        items = new List<Item>(itemAnchors.Count); // Inicializa la lista de objetos
        state = State.Idle;
        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs   // Llamar a la UI Progress
        {
            progressNormalized = 0    // El Fill depende del tiempo de cocinar y el tiempo cocinado
        });
    }

    public override void InteractPick(PlayerInteraction owner, Item playerItem)
    {
        if (state == State.Idle) // Solo permite agregar ingredientes cuando está en estado Idle
        {
            if (playerItem != null) // Si el jugador tiene un objeto
            {
                if (GrabItem(playerItem)) // Intentar agregar el objeto al caldero
                {
                    owner.DropItem(); // El jugador suelta el objeto
                    Debug.Log("Ingrediente agregado al caldero");

                    if (items.Count == itemAnchors.Count) // Si el caldero está lleno
                    {
                        StartCooking(); // Iniciar el proceso de cocción
                    }
                }
                else
                {
                    Debug.Log("No se puede agregar el objeto al caldero.");
                }
            }
            else
            {
                Debug.Log("No hay objeto para agregar.");
            }
        }
        else
        {
            Debug.Log("El caldero está en proceso de cocción, no se pueden agregar ingredientes.");
        }
    }

    private void StartCooking()
    {
        state = State.Cooking; // Cambia al estado Cooking
        cookingTimer = 0f; // Restablece el temporizador
        Debug.Log("El caldero está cocinando...");
    }

    private void Update()
    {
        if (state == State.Cooking) // Solo incrementa el temporizador si el caldero está cocinando
        {
            cookingTimer += Time.deltaTime;

            OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs   // Llamar a la UI Progress
            {
                progressNormalized = cookingTimer / cookingTime   // El Fill depende del tiempo de cocinar y el tiempo cocinado
            });

            if (cookingTimer >= cookingTime) // Si el temporizador alcanza el tiempo máximo
            {
                OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs   // Llamar a la UI Progress
                {
                    progressNormalized = 0   // El Fill depende del tiempo de cocinar y el tiempo cocinado
                });
                CompleteCooking(); // Completar el proceso de cocción
            }
        }
    }

    private void CompleteCooking()
    {
        // Destruye los ingredientes anteriores
        foreach (var item in items)
        {
            if (item != null)
            {
                item.GetComponent<Ingredient>().DestroySelf();
            }
        }

        items.Clear(); // Limpia la lista de ingredientes
        Ingredient.SpawnIngredientObject(potionResult, this.transform); // Instanciar el resultado final (Poción)   //

        state = State.Completed; // Cambia al estado Completed
        Debug.Log("El caldero ha completado la cocción y ha creado una poción.");
    }

    protected override bool GrabItem(Item newItem)
    {
        // Verificar si el objeto es un ingrediente válido
        IngredientData ingredientData = newItem.GetComponent<Ingredient>().GetIngredientData();
        if (ingredientData == null || !validIngredientDataList.Contains(ingredientData))
        {
            Debug.Log("El objeto no es un ingrediente válido para el caldero.");
            return false; // El objeto no es válido
        }

        // Encuentra el primer punto de anclaje disponible
        for (int i = 0; i < itemAnchors.Count; i++)
        {
            if (items.Count <= i || items[i] == null) // Si el anclaje está vacío
            {
                newItem.transform.SetParent(itemAnchors[i], false); // Establece el padre
                newItem.transform.localPosition = Vector3.zero;

                if (items.Count <= i)
                {
                    items.Add(newItem); // Añadir a la lista si es nuevo
                }
                else
                {
                    items[i] = newItem; // Reemplazar el objeto en esa posición
                }

                return true; // Indicar que el objeto fue agregado con éxito
            }
        }

        return false; // No hay espacio disponible
    } */

    public enum State
    {
        Idle,
        Cooking,
        Completed
    }

    public event EventHandler<UIProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [Header("Inspector")]
    [SerializeField] private List<IngredientData> validIngredientDataList;
    [SerializeField] private RecipeListData recipeList; // Lista de todas las recetas
    [SerializeField] private List<Transform> itemAnchors;
    //[SerializeField] private float cookingTime; // Tiempo máximo para cocinar

    private List<Item> items;
    private State state;
    private float cookingTimer;
    [SerializeField] private RecipeData activeRecipe; // La receta activa


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
        else
        {
            Debug.Log("El caldero está cocinando.");
        }
    }

    private void StartCooking()
    {
        state = State.Cooking; // Cambia al estado Cooking
        cookingTimer = 0f; // Restablece el temporizador

        var calderoIngredients = new List<IngredientData>();
        foreach (var item in items)
        {
            var ingredientData = item.GetComponent<Ingredient>().GetIngredientData();
            if (ingredientData != null)
            {
                calderoIngredients.Add(ingredientData);
            }
        }

        activeRecipe = recipeList.FindMatchingRecipe(calderoIngredients); // Encuentra la receta correcta
        if (activeRecipe == null)
        {
            Debug.LogError("No se encontró receta para los ingredientes actuales.");
            state = State.Idle; 
            return;
        }

        Debug.Log("El caldero está cocinando...");
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
        Ingredient.SpawnIngredientObject(activeRecipe.result, itemAnchors[0]); // Instanciar el resultado final

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

        // Encuentra el primer punto de anclaje disponible
        for (int i = 0; i < itemAnchors.Count; i++)
        {
            if (items.Count <= i || items[i] == null) // Si el anclaje está vacío
            {
                newItem.transform.SetParent(itemAnchors[i], false); // Establece el padre
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
}
