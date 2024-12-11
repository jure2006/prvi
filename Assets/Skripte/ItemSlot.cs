using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    // Get the item in the slot, if any
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                var child = transform.GetChild(0).gameObject;
                // Ensure the child has the InventoryItem component
                if (child.GetComponent<InventoryItem>())
                {
                    return child;
                }
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        // If there is no item in the slot, add the dragged item
        if (!Item)
        {
            var draggedItem = DragDrop.itemBeingDragged;
            draggedItem.transform.SetParent(transform);
            draggedItem.transform.localPosition = Vector3.zero;  // Align the dragged item to (0, 0)

            // Check if the slot is a QuickSlot
            if (!transform.CompareTag("QuickSlot"))
            {
                draggedItem.GetComponent<InventoryItem>().isNowEquipped = false;
            }
            else
            {
                draggedItem.GetComponent<InventoryItem>().isNowEquipped = true;
            }
        }
    }
}