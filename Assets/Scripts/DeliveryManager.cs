using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private PotionListData potionListData;
    private List<IngredientData> waitingPotionDataList;
    private float spawnPotionTimer;
    private float spawnPotionTimerMax = 4f;
    private int waitingPotionMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingPotionDataList = new List<IngredientData>();
    }

    private void Update()
    {
        spawnPotionTimer -= Time.deltaTime;
        if (spawnPotionTimer <= 0f)
        {
            spawnPotionTimer = spawnPotionTimerMax;

            if (waitingPotionDataList.Count < waitingPotionMax)
            {
                IngredientData waitingPotionData = potionListData.potionListData[Random.Range(0, potionListData.potionListData.Count)];
                Debug.Log(waitingPotionData.itemName);
                waitingPotionDataList.Add(waitingPotionData);
            }
        }
    }

    public void DeliveryPotion(Ingredient ingredient)
    {
        for (int i = 0; i < waitingPotionDataList.Count; i++)
        {
            IngredientData waitingPotionData = waitingPotionDataList[i];
            Debug.Log(waitingPotionData.itemName);
            Debug.Log(ingredient.GetIngredientData());

            if (ingredient.GetIngredientData() == waitingPotionData)
            {
                Debug.Log("La orden y la pocion coinciden");
                waitingPotionDataList.RemoveAt(i);
            }
            if (ingredient.GetIngredientData() != waitingPotionData)
            {
                Debug.Log("La orden y la pocion no coinciden");
            }
        }
        if (waitingPotionDataList.Count == 0)
        {
            Debug.Log("El jugador completo todas las recetas");

        }
    }
}
