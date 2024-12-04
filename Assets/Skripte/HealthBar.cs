using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour{
private Slider slider;
public TMP_Text healthCounter;
public GameObject PlayerState;

private float currentHealth, maxHealth; 


    void Awake()
    {
        slider = GetComponent<Slider>();
    }



    void Start()
{
    if (PlayerState != null)
    {
        currentHealth = PlayerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = PlayerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
        healthCounter.text = $"{currentHealth}/{maxHealth}";
    }
}

    void Update()
    { currentHealth= PlayerState.GetComponent<PlayerState>().currentHealth;
    maxHealth= PlayerState.GetComponent<PlayerState>().maxHealth;


    float fillValue = currentHealth / maxHealth; 
    slider.value = fillValue;
    healthCounter.text = currentHealth + "/" + maxHealth;
     
        
    }





}
