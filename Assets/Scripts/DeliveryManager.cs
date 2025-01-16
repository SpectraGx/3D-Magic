using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnPotionSpawned;
    public event EventHandler OnPotionCompleted;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private PotionListData potionListData;
    private List<IngredientData> waitingPotionDataList;
    private float spawnPotionTimer;
    private float spawnPotionTimerMax = 4f;
    private int waitingPotionMax = 4;
    private int successfulPotionsAmounts;

    [SerializeField] private DeliveryTable deliveryTable;
    [SerializeField] private float particlesDuration=1f;
    [SerializeField] private AudioClip potionMatch;
    [SerializeField] private AudioClip potionMismatch;
    private AudioSource audioSource;
    

    private Coroutine particlesCoroutine;
    private Coroutine particlesSpiralsCoroutine;


    private void Awake()
    {
        Instance = this;
        waitingPotionDataList = new List<IngredientData>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        spawnPotionTimer -= Time.deltaTime;
        if (spawnPotionTimer <= 0f)
        {
            spawnPotionTimer = spawnPotionTimerMax;

            if (waitingPotionDataList.Count < waitingPotionMax)
            {
                IngredientData waitingPotionData = potionListData.potionListData[UnityEngine.Random.Range(0, potionListData.potionListData.Count)];
                //Debug.Log(waitingPotionData.itemName);
                waitingPotionDataList.Add(waitingPotionData);

                OnPotionSpawned?.Invoke(this, EventArgs.Empty);
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
                //Debug.Log("La orden y la pocion coinciden");
                OnPotionCompleted?.Invoke(this, EventArgs.Empty);
                successfulPotionsAmounts++;
                waitingPotionDataList.RemoveAt(i);
                if (deliveryTable != null){
                    deliveryTable.ActivateDeliveryParticles();
                    if (particlesCoroutine != null)
                        StopCoroutine(particlesCoroutine);
                    particlesCoroutine = StartCoroutine(DesactivateParticlesDelay());
                    
                }
                PlayMatchPotion();


                break;
            }
            else {
                Debug.Log("No coinciden");
                PlayMismatchPotion();
                if (deliveryTable != null){
                    deliveryTable.ActivateSpiralsParticles();
                    if (particlesSpiralsCoroutine != null)
                        StopCoroutine(particlesSpiralsCoroutine);
                    particlesSpiralsCoroutine = StartCoroutine(DesactivateParticlesSpiralsDelay());
                    
                }
            }
            /* if (ingredient.GetIngredientData() != waitingPotionData)
            {
                Debug.Log("La orden y la pocion no coinciden");
            } */
        }



        if (waitingPotionDataList.Count == 0)
        {
            Debug.Log("El jugador completo todas las recetas");

        }
    }

    public List<IngredientData> GetWaitingPotionDataList()
    {
        return waitingPotionDataList;
    }

    public int GetSuccessfulPotionsAmounts(){
        return successfulPotionsAmounts;
    }

    private IEnumerator DesactivateParticlesDelay(){
        yield return new WaitForSeconds(particlesDuration);
        if (deliveryTable!=null){
            deliveryTable.DesactivateDeliveryParticles();
        }
    }

    private IEnumerator DesactivateParticlesSpiralsDelay(){
        yield return new WaitForSeconds(particlesDuration);
        if (deliveryTable!=null){
            deliveryTable.DeactivateSpiralsParticles();
        }
    }

    private void PlayMatchPotion()
    {
        if (potionMatch != null && audioSource != null)
        {
            audioSource.clip = potionMatch;
            audioSource.Play();
        }
    }

    private void PlayMismatchPotion()
    {
        if (potionMismatch != null && audioSource != null)
        {
            audioSource.clip = potionMismatch;
            audioSource.Play();
        }
    }

    private void StopCuttingSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
