using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject selectedObject;

    public bool onTarget;

    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;


    public Image centerDotIcon;
    public Image handIcon; 



    private void Start()
    {
        onTarget = false;

        // Preveri, če je interaction_Info_UI nastavljen
        if (interaction_Info_UI != null)
        {
            // Poskusi pridobiti TMP_Text komponento, če obstaja
            interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();

            if (interaction_text == null)
            {
                Debug.LogError("TMP_Text komponenta ni najdena na interaction_Info_UI!");
            }
        }
        else
        {
            Debug.LogError("interaction_Info_UI ni nastavljen v Inspectorju!");
        }
    }

    private void Awake()
    {
        // Singleton nastavitev z varnim preverjanjem
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        // Preveri, če je interaction_Info_UI nastavljen, da se izogneš napakam
        if (interaction_Info_UI == null)
        {
            Debug.LogWarning("interaction_Info_UI ni nastavljen, preskakujem logiko!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable  && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;


                    interaction_text.text = interactable.GetItemName();

                

                interaction_Info_UI.SetActive(true);

                if(interactable.CompareTag("Pickable")){
                      
                  centerDotIcon.gameObject.SetActive(false);
                  handIcon.gameObject.SetActive(true);

                }
                else
                {
                       handIcon.gameObject.SetActive(false);
                     centerDotIcon.gameObject.SetActive(true);
                  



                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                     centerDotIcon.gameObject.SetActive(true);
                  

            }
        }
        else
        {
            onTarget= false;
            interaction_Info_UI.SetActive(false);
            handIcon.gameObject.SetActive(false);
                     centerDotIcon.gameObject.SetActive(true);
                  

        }
    }
}
