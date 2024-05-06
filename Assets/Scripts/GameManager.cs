using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
   /*  [Header("Singleton")]
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum GameState {
        PreGame,
        InGame,
        GameOver
    }

    public EventHandler<int> OnCurrencyChange;
    public EventHandler<float> OnTimeChange;
    public EventHandler<RecipeData> OnOrderCreate;
    public EventHandler OnOrderSuccess;

    [Header("SerializedField")]
    [SerializeField] private float preGameTime;
    [SerializeField] private float gameTime;
    [SerializeField] private List<RecipeData> recipeDataList = new List<RecipeData>();

    // Private *****
    private GameState gameState;
    private float stateTimer;
    private float timeHelper;
    private float orderTime;
    private int coins;

    private List<RecipeData> activeRecipes = new List<RecipeData>();

    private void Start()
    {
        // Idle time
        stateTimer = preGameTime;
        coins = 0;
        OnTimeChange?.Invoke(this, gameTime);
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        timeHelper -= Time.deltaTime;
        orderTime -= Time.deltaTime;

        switch (gameState)
        {
            case GameState.PreGame:
                if (stateTimer <= 0)
                {
                    stateTimer = gameTime;
                    gameState = GameState.InGame;
                }
                break;
            case GameState.InGame:
                if (timeHelper <= 0)
                {
                    OnTimeChange?.Invoke(this, stateTimer);
                    timeHelper = 1;
                }

                if (orderTime <= 0 && recipeDataList.Count > 0)
                {
                    RecipeData newOrder = recipeDataList[0];
                    activeRecipes.Add(newOrder);
                    OnOrderCreate?.Invoke(this,newOrder);
                    orderTime = 35;
                }
                if (stateTimer <= 0)
                {
                    SetGameOver();
                }
                break;
        }
        

    }
    
    public void SetGameOver()
    {
        gameState = GameState.GameOver;
    }

    public void SuccessOrder()
    {
        OnOrderSuccess?.Invoke(this, EventArgs.Empty);
    } */
}
