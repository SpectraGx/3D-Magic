using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : Tile, UIProgress
{
    public event EventHandler<UIProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [Header("Inspector")]
    [SerializeField] private float timeToCut = 3f;
    [Header("Variables privadas")]
    private float cutTimer = 0f;
    private bool _isCutting = false;

    const string Player_Cut = "player_cutting";
    const string Player_Idle = "player_idle";



    public override void OnInteractStart(PlayerInteraction player)
    {
        if (item != null && item.TryGetComponent(out Ingredient ingredient) && ingredient.CanBeCut())
        {
            _isCutting = true;  // Empieza a cortar
            player.playerController.ChangeAnimationState(Player_Cut);
        }
    }

    public override void OnInteractStop(PlayerInteraction player)
    {
        _isCutting = false;     // Deja de cortar
        cutTimer = 0;
        player.playerController.ChangeAnimationState(Player_Idle);


        OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
        {
            progressNormalized = cutTimer
        });
    }

    private void Update()
    {
        if (_isCutting) // Si esta cortando
        {
            cutTimer += Time.deltaTime; // Timer de cortar

            OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs   // Llamar a la UI Progress
            {
                progressNormalized = cutTimer / timeToCut   // El Fill depende del tiempo que ha presionado el jugador y el tiempo para cortar
            });

            Debug.Log($"Tiempo de corte: {cutTimer:F2} segundos");
            if (cutTimer >= timeToCut)  // Si se mantuvo cortando la cantidad de tiempo necesitada
            {
                OnProgressChanged?.Invoke(this, new UIProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = timeToCut
                });
                // Remplazar por objeto cortado
                if (item.TryGetComponent(out Ingredient ingredient) && CraftFood(ingredient.GetNextIngredientData()))
                {
                    Debug.Log("El ingrediente fue remplazado por uno cortado");
                    _isCutting = false; // Reseteo de corte
                    cutTimer = 0f;      // Reincio de timer
                }
            }
        }
    }


    public override void InteractPick(PlayerInteraction owner, Item playerItem)
    {
        base.InteractPick(owner, playerItem);
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
