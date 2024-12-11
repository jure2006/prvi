using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; private set; }

    // -- UI Elements -- //
    public GameObject quickSlotsPanel;
    public GameObject numberHolder;

    // Lists for slots and items
    private readonly List<GameObject> quickSlotsList = new List<GameObject>();
    private readonly List<string> itemList = new List<string>();

    // Selected slot/item tracking
    public int selectedNumber = -1;
    public GameObject selectedItem;

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
        PopulateSlotList();
        // Set initial color of all numbers to gray
        SetAllSlotNumbersGray();
    }

    private void Update()
    {
        // Handle slot selection with numeric keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectQuickSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectQuickSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectQuickSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SelectQuickSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SelectQuickSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) SelectQuickSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) SelectQuickSlot(7);
    }

    private void SelectQuickSlot(int number)
    {
        if (!CheckIfSlotIsFull(number)) return;

        // If a new slot is selected
        if (selectedNumber != number)
        {
            selectedNumber = number;

            // Deselect the currently selected item
            if (selectedItem != null)
            {
                var inventoryItem = selectedItem.GetComponent<InventoryItem>();
                if (inventoryItem != null)
                {
                    inventoryItem.isSelected = false;
                }
            }

            // Select the new item
            selectedItem = GetSelectedItem(number);
            if (selectedItem != null)
            {
                var newInventoryItem = selectedItem.GetComponent<InventoryItem>();
                if (newInventoryItem != null)
                {
                    newInventoryItem.isSelected = true;
                }
            }

            // Update UI to highlight the selected slot
            UpdateSlotUI(number);
        }
        else
        {
            // Deselect current slot if the same number is pressed again
            DeselectCurrentSlot();
        }
    }

    private void DeselectCurrentSlot()
    {
        selectedNumber = -1;

        if (selectedItem != null)
        {
            var inventoryItem = selectedItem.GetComponent<InventoryItem>();
            if (inventoryItem != null)
            {
                inventoryItem.isSelected = false;
            }
        }

        // Reset all slot numbers to gray
        SetAllSlotNumbersGray();

        selectedItem = null;
    }

    private GameObject GetSelectedItem(int slotNumber)
    {
        if (slotNumber - 1 < quickSlotsList.Count && quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
        }

        Debug.LogWarning($"Quick slot {slotNumber} is empty or out of range.");
        return null;
    }

    private bool CheckIfSlotIsFull(int slotNumber)
    {
        if (slotNumber - 1 < quickSlotsList.Count)
        {
            return quickSlotsList[slotNumber - 1].transform.childCount > 0;
        }

        Debug.LogWarning($"Slot number {slotNumber} is out of range.");
        return false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();

        if (availableSlot == null)
        {
            Debug.LogWarning("No available quick slot found!");
            return;
        }

        itemToEquip.transform.SetParent(availableSlot.transform, false);

        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return null;
    }

    public bool CheckIfFull()
    {
        int occupiedSlots = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                occupiedSlots++;
            }
        }

        return occupiedSlots == quickSlotsList.Count;
    }

    // Set the color of all slot numbers to gray
    private void SetAllSlotNumbersGray()
    {
        foreach (Transform child in numberHolder.transform)
        {
            TMP_Text tmpText = child.GetComponentInChildren<TMP_Text>(); // Use GetComponentInChildren
            if (tmpText != null)
            {
                tmpText.color = Color.gray;
            }
            else
            {
                Debug.LogWarning($"TMP_Text komponenta ni bila najdena za child {child.name}!");
            }
        }
    }

    // Update the color of the selected slot to white
    private void UpdateSlotUI(int selectedSlotNumber)
    {
        // Set all numbers to gray first
        SetAllSlotNumbersGray();

        // Find the selected slot by its name (number1, number2, ...)
        Transform selectedSlot = numberHolder.transform.Find($"number{selectedSlotNumber}");
        if (selectedSlot != null)
        {
            TMP_Text selectedSlotText = selectedSlot.Find("Text")?.GetComponent<TMP_Text>();
            if (selectedSlotText != null)
            {
                selectedSlotText.color = Color.white;
            }
            else
            {
                Debug.LogWarning($"TMP_Text komponenta za {selectedSlotNumber} ni bila najdena!");
            }
        }
        else
        {
            Debug.LogWarning($"Slot za {selectedSlotNumber} ni bil najden v numberHolder!");
        }
    }
}
