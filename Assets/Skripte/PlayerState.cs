using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // Health
    public float currentHealth;
    public float maxHealth;

    // Calories
    public float currentCalories;
    public float maxCalories;

    private float distanceTravelled = 0;
    private Vector3 lastPosition;

    public GameObject playerBody;

    // Hydration
    public float currentHydrationPercent;
    public float maxHydrationPercent;
    public bool isHydrationActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Initializing the player's health, calories, and hydration
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        // Start the hydration decrease coroutine
        StartCoroutine(DecreaseHydration());
    }

    // Coroutine to decrease hydration over time
    IEnumerator DecreaseHydration()
    {
        while (true)
        {
            if (isHydrationActive)
            {
                currentHydrationPercent -= 1;
                currentHydrationPercent = Mathf.Clamp(currentHydrationPercent, 0, maxHydrationPercent); // Prevents negative hydration
            }
            yield return new WaitForSeconds(2);
        }
    }

    void Update()
    {
        // Track distance travelled to decrease calories
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;  // Decrease calories when player moves
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;  // Decrease health for testing purposes
        }
    }

    // Setter for Health
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth); // Clamps the value between 0 and maxHealth
    }

    // Setter for Calories
    public void SetCalories(float calories)
    {
        currentCalories = Mathf.Clamp(calories, 0, maxCalories); // Clamps the value between 0 and maxCalories
    }

    // Setter for Hydration
    public void SetHydration(float hydration)
    {
        currentHydrationPercent = Mathf.Clamp(hydration, 0, maxHydrationPercent); // Clamps the value between 0 and maxHydrationPercent
    }
}