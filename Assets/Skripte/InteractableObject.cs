using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

void Update(){
    if(Input.GetKeyDown(KeyCode.Mouse0)&& playerInRange&& SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject==gameObject)
    {
         //če ni poln  naredi to 
        if(!InventorySystem.Instance.CheckifFull())
        {
            
        InventorySystem.Instance.AddTooInventory(ItemName);

        Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full");
        }

    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}