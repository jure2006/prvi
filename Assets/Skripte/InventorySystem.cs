using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isOpen;

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

    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddTooInventory(string ItemName)
    {
        whatSlotToEquip = FindNextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>(ItemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(ItemName);
    }

    public bool CheckifFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
        return counter == 28;
    }

    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    //tilen was here
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {

        int counter = amountToRemove;
        for (var i = slotList.Count -1; i>=0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove +"(Clone)" && counter !=0)
                {

                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;

                }


            }

        }


    }

    public void ReCalculateList()
    {

        itemList.Clear();
        foreach(GameObject slot in slotList)
        {

            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; //stone clone
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }




}