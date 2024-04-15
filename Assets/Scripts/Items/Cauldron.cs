using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Item
{
    
    // Public *****
    public enum CauldronState
    {
        Empty, // Applies if its not full
        RawValidRecipe, 
        Cooking, // Change to this state depends on Stove
        Cooked,
        CookedCooking,
        Burned
    }
    // Public *****
    public EventHandler<Cauldron> OnFullAndValidRecipe;
    
    // Serialized
    [Header("Cauldron")]
    [SerializeField] private int maxCapacity = 1;
    // TODO: Change this to a list
    [SerializeField] private RecipeData recipe;
    [SerializeField] private ParticleSystem steamParticles;
    
    [Header("Pot")]
    //TODO: CHANGE TO a specific pot class
    [SerializeField] private Transform potSoup;
    [SerializeField] private Vector2 yMinMaxPotSoup;
    //[SerializeField] private ProgressUI _progressUI;

    // Private *****
    private CauldronState _CauldronState;
    private List<IngredientData> _IngredientDataList = new List<IngredientData>();
    private float _CauldronTimer;

    // MonoBehavior Callbacks *****

    private void Start()
    {
        potSoup.position = new Vector3(potSoup.position.x, yMinMaxPotSoup.x, potSoup.position.z);
    }

    // Private Methods *****
    private void Update()
    {
        if (!IsCooking()) return;
        
        _CauldronTimer -= Time.deltaTime;

        if (_CauldronState == CauldronState.Cooking)
        {
            OnItemProgressChange?.Invoke(this, _CauldronTimer/ recipe.cookingTime);
        }
        
        if (!(_CauldronTimer <= 0)) return;
        
        switch (_CauldronState)
        {
            case CauldronState.Cooking:
                steamParticles.Play();
                _CauldronState = CauldronState.CookedCooking;
                _CauldronTimer = 5; // 5 seconds to burn
                break;
            case CauldronState.CookedCooking:
                Debug.Log("your comnida is burn");
                _CauldronState = CauldronState.Burned;
                break;
        }
    }
    
    // Private MEthods *****
    private float GetSoupLevel()
    {
        float percentage = (float)_IngredientDataList.Count / maxCapacity;
        float limit = yMinMaxPotSoup.x + yMinMaxPotSoup.y;

        return percentage * limit;
    }


    // Public Methods *****
    public bool AddFood(IngredientData IngredientData)
    {
        if (_IngredientDataList.Count >= maxCapacity || IngredientData.ingredientType != IngredientType.Processed) return false;
        
        _IngredientDataList.Add(IngredientData);
        potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);
        
        //CauldronFull
        if (_IngredientDataList.Count >= maxCapacity)
        {
            // TODO: Check if is a valid recipe
            _CauldronState = CauldronState.RawValidRecipe;
            OnFullAndValidRecipe?.Invoke(this, this);
        }
        
        return true;
    }

    public void StartCook()
    {
        if(_CauldronState == CauldronState.RawValidRecipe)
        {
            _CauldronState = CauldronState.Cooking;
            _CauldronTimer = recipe.cookingTime;
            
            /*
            ProgressUI progressUI = Instantiate(_progressUI);
            progressUI.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasInteraction").transform);
            progressUI.Set(this);
            */
            
        }
        else if (_CauldronState == CauldronState.Cooked)
        {
            _CauldronState = CauldronState.CookedCooking;
            _CauldronTimer = 5f;
        }
    }

    public void StopCook()
    {
        if(_CauldronState == CauldronState.Cooking) _CauldronState = CauldronState.RawValidRecipe;
        else if (_CauldronState == CauldronState.CookedCooking) _CauldronState = CauldronState.Cooked;
    }

    public bool CanBeCooked() =>
        (_CauldronState == CauldronState.RawValidRecipe || _CauldronState == CauldronState.Cooked);
    
    private bool IsCooking() => (_CauldronState == CauldronState.Cooking || _CauldronState == CauldronState.CookedCooking);

    public bool TryGetDish(out IngredientData foodCooked)
    {
        if (_CauldronState == CauldronState.Cooked)
        {
            foodCooked = recipe.result;
            _IngredientDataList.Clear();
            _CauldronState = CauldronState.Empty;
            steamParticles.Stop();
            potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);
            return true;
        }

        foodCooked = null;
        return false;
    }
}
