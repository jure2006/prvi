using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public TMP_Text HydrationCounter;
    public GameObject PlayerState;

    private float currentHydration, maxHydration; 

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        if (PlayerState != null)
        {
            currentHydration = PlayerState.GetComponent<PlayerState>().currentHydrationPercent;
            maxHydration = PlayerState.GetComponent<PlayerState>().maxHydrationPercent;

            float fillValue = currentHydration / maxHydration;
            slider.value = fillValue;
            HydrationCounter.text = $"{currentHydration}/{maxHydration}";
        }
    }

    void Update()
    { 
        currentHydration = PlayerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydration = PlayerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currentHydration / maxHydration; 
        slider.value = fillValue;
        HydrationCounter.text = $"{currentHydration}/{maxHydration}";
    }
}