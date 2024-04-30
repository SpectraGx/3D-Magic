using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Item
{

    //          ESTADOS DEL CALDERO
    public enum CauldronState
    {
        Empty,              // Si no esta llena
        RawValidRecipe,
        Cooking,            // Cambio de estado dependiendo la fogata
        Cooked,
        CookedCooking,
        Burned
    }
    [Header("Evento publico")]
    public EventHandler<Cauldron> OnFullAndValidRecipe;

    [Header("Bonfire")]
    [SerializeField] private int maxCapacity = 1;
    // CAMBIAR A LISTA
    [SerializeField] private RecipeData recipe;
    [SerializeField] private ParticleSystem steamParticles;

    [Header("Cauldron")]
    // CAMBIAR A CLASE DE CALDERO
    [SerializeField] private Transform potSoup;
    [SerializeField] private Vector2 yMinMaxPotSoup;
    //[SerializeField] private ProgressUI _progressUI;

    [Header("Variables privadas")]
    private CauldronState cauldronState;
    private List<IngredientData> ingredientsDataList = new List<IngredientData>();
    private float cauldronTimer;


    private void Start()
    {
        potSoup.position = new Vector3(potSoup.position.x, yMinMaxPotSoup.x, potSoup.position.z);
    }

    private void Update()
    {
        if (!IsCooking()) return;

        cauldronTimer -= Time.deltaTime;

        if (cauldronState == CauldronState.Cooking)
        {
            OnItemProgressChange?.Invoke(this, cauldronTimer / recipe.cookingTime);
        }

        if (!(cauldronTimer <= 0)) return;

        switch (cauldronState)
        {
            case CauldronState.Cooking:
                steamParticles.Play();
                cauldronState = CauldronState.CookedCooking;
                cauldronTimer = 5; // 5 seconds to burn
                break;
            case CauldronState.CookedCooking:
                Debug.Log("La poción ya no es bebible");
                cauldronState = CauldronState.Burned;
                break;
        }
    }

    //          METODO PRIVADO
    private float GetSoupLevel()
    {
        float percentage = (float)ingredientsDataList.Count / maxCapacity;
        float limit = yMinMaxPotSoup.x + yMinMaxPotSoup.y;

        return percentage * limit;
    }


    //          METODOS PUBLICOS
    public bool AddFood(IngredientData ingredientData)
    {
        if (ingredientsDataList.Count >= maxCapacity || ingredientData.ingredientType != IngredientType.Processed) return false;

        ingredientsDataList.Add(ingredientData);
        potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);

        // Si el caldero esta lleno
        if (ingredientsDataList.Count >= maxCapacity)
        {
            // Comprueba que sea una poción valida
            cauldronState = CauldronState.RawValidRecipe;
            OnFullAndValidRecipe?.Invoke(this, this);
        }

        return true;
    }

    public void StartCook()
    {
        if (cauldronState == CauldronState.RawValidRecipe)
        {
            cauldronState = CauldronState.Cooking;
            cauldronTimer = recipe.cookingTime;

            //ProgressUI progressUI = Instantiate(_progressUI);
            //progressUI.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasInteraction").transform);
            //progressUI.Set(this);

        }
        else if (cauldronState == CauldronState.Cooked)
        {
            cauldronState = CauldronState.CookedCooking;
            cauldronTimer = 5f;
        }
    }

    public void StopCook()
    {
        if (cauldronState == CauldronState.Cooking) cauldronState = CauldronState.RawValidRecipe;
        else if (cauldronState == CauldronState.CookedCooking) cauldronState = CauldronState.Cooked;
    }

    public bool CanBeCooked() =>
        (cauldronState == CauldronState.RawValidRecipe || cauldronState == CauldronState.Cooked);

    private bool IsCooking() => (cauldronState == CauldronState.Cooking || cauldronState == CauldronState.CookedCooking);

    public bool TryGetDish(out IngredientData potionPrepared)
    {
        if (cauldronState == CauldronState.Cooked)
        {
            potionPrepared = recipe.result;
            ingredientsDataList.Clear();
            cauldronState = CauldronState.Empty;
            steamParticles.Stop();
            potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);
            return true;
        }

        potionPrepared = null;
        return false;
    }
}
