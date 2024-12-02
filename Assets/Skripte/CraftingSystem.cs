using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;


public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN;

    //Craft Buttons
    Button craftAxeBTN;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2;

    public bool isOpen;

    //All Blueprints
    public Blueprint  AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);




    public static CraftingSystem Instance { get; set; }


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


    // Start is called before the first frame update
    void Start()
    {

        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }


    void CraftAnyItem(Blueprint blueprintToCrafting)
    { //tilen was here 39:33
        InventorySystem.Instance.AddTooInventory(blueprintToCrafting.itemName);
        
        if(blueprintToCrafting.numOfRequirements==1)
        {
           InventorySystem.Instance.RemoveItem(blueprintToCrafting.Req1, blueprintToCrafting.Req1amount); 
        }
        else if(blueprintToCrafting.numOfRequirements==2){

             
        InventorySystem.Instance.RemoveItem(blueprintToCrafting.Req1, blueprintToCrafting.Req1amount);
        InventorySystem.Instance.RemoveItem(blueprintToCrafting.Req2, blueprintToCrafting.Req2amount); 
        }

        

        InventorySystem.Instance.ReCalculateList();
         

         
         StartCoroutine(calculate());
        RefreshNeededItems();
         

         
        //do sem tilen was here 


        //add item into inventory


        //remove resources from inventory


    }


            public IEnumerator calculate()
            {
                yield return new WaitForSeconds(1f);
                InventorySystem.Instance.ReCalculateList();
            }      



    // Update is called once per frame
    void Update()
    {

        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }



    }
    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {

            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;

                case "Stick":
                    stick_count += 1;
                    break;
            }


        }
        // A X E //
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

            if(stone_count >=3 && stick_count >=3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
            else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }



    }


}  