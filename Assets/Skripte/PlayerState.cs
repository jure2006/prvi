using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance {get; set;}
    
//healt
public float currentHealth;
public float maxHealth;




// calories

public float currentCalories;
public float maxCalories;

float distanceTravelled = 0;
Vector3 lastPostiton;

public GameObject playerBody;



// hydration
public float currentHydrationPercent;
public float maxHydrationPercent;
public bool isHydrationActive;


   private void Awake(){
         if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


private void Start(){


    currentHealth = maxHealth;
    currentCalories = maxCalories;
    currentHydrationPercent = maxHydrationPercent;


     StartCoroutine(decreaseHydration());


}
IEnumerator decreaseHydration(){
    while(true){
        currentHydrationPercent -=1;
        yield return new WaitForSeconds(2);
    }
}




    // Update is called once per frame
    void Update()
    {

       distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPostiton);
       lastPostiton = playerBody.transform.position;

       if(distanceTravelled >=5){

        distanceTravelled =0;
        currentCalories -= 1;

       }


     if (Input.GetKeyDown(KeyCode.N))
     {
        currentHealth -= 10;
        }   
    }
}
