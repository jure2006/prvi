using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAndDrop : MonoBehaviour
{
    public GameObject camera;
    float maxpickupdistance = 5;
    GameObject itemcurrentlyholding;
    bool isholding = false;

    // Dictionary to track original positions of items
    private Dictionary<GameObject, Vector3> itemInitialPositions = new Dictionary<GameObject, Vector3>();

    // Dictionary to track items' respawn timers
    private Dictionary<GameObject, Coroutine> itemRespawnCoroutines = new Dictionary<GameObject, Coroutine>();

    public void Start()
    {
        // Initialize the dictionary with all pickable items
        GameObject[] pickableItems = GameObject.FindGameObjectsWithTag("Pickable");
        foreach (GameObject item in pickableItems)
        {
            itemInitialPositions[item] = item.transform.position;

            // Disable gravity initially for items at their starting positions
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true; // Ensure kinematic is enabled at the start
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    public void Pickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxpickupdistance))
        {
            if (hit.transform.tag == "Pickable")
            {
                if (isholding) Drop();

                itemcurrentlyholding = hit.transform.gameObject;

                // Cancel respawn if it's in progress
                if (itemRespawnCoroutines.ContainsKey(itemcurrentlyholding) && itemRespawnCoroutines[itemcurrentlyholding] != null)
                {
                    StopCoroutine(itemRespawnCoroutines[itemcurrentlyholding]);
                    itemRespawnCoroutines[itemcurrentlyholding] = null;
                }

                foreach (var c in hit.transform.GetComponentsInChildren<Collider>()) if (c != null) c.enabled = false;
                foreach (var r in hit.transform.GetComponentsInChildren<Rigidbody>()) if (r != null)
                {
                    r.isKinematic = true; // Ensure it's kinematic while holding
                    r.useGravity = false;
                }

                itemcurrentlyholding.transform.parent = transform;
                itemcurrentlyholding.transform.localPosition = Vector3.zero;
                itemcurrentlyholding.transform.localEulerAngles = Vector3.zero;

                isholding = true;

                Debug.Log("Picked up: " + itemcurrentlyholding.name);
            }
        }
    }

    public void Drop()
    {
        if (itemcurrentlyholding == null) return;

        itemcurrentlyholding.transform.parent = null;
        foreach (var c in itemcurrentlyholding.GetComponentsInChildren<Collider>()) if (c != null) c.enabled = true;
        foreach (var r in itemcurrentlyholding.GetComponentsInChildren<Rigidbody>()) if (r != null)
        {
            r.isKinematic = false; // Allow physics interaction
            r.useGravity = true; // Enable gravity when dropped
        }

        isholding = false;

        RaycastHit hitDown;
        if (Physics.Raycast(transform.position, -Vector3.up, out hitDown))
        {
            itemcurrentlyholding.transform.position = hitDown.point + new Vector3(transform.forward.x, 0, transform.forward.z);
        }

        // Start respawn timer
        if (itemInitialPositions.ContainsKey(itemcurrentlyholding))
        {
            Coroutine respawnCoroutine = StartCoroutine(RespawnItemAfterDelay(itemcurrentlyholding, 10f));
            itemRespawnCoroutines[itemcurrentlyholding] = respawnCoroutine;
        }

        Debug.Log("Dropped: " + itemcurrentlyholding.name);

        itemcurrentlyholding = null;
    }

    private IEnumerator RespawnItemAfterDelay(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Respawn the item at its original position
        if (itemInitialPositions.ContainsKey(item))
        {
            item.transform.position = itemInitialPositions[item];
            item.transform.rotation = Quaternion.identity; // Reset rotation to default

            foreach (var c in item.GetComponentsInChildren<Collider>()) if (c != null) c.enabled = true;
            foreach (var r in item.GetComponentsInChildren<Rigidbody>()) if (r != null)
            {
                r.isKinematic = true; // Disable physics at initial position
                r.useGravity = false; // Turn off gravity at initial position
            }

            Debug.Log("Respawned: " + item.name + " at " + itemInitialPositions[item]);
        }
        else
        {
            Debug.LogWarning("Item not found in initial positions dictionary: " + item.name);
        }

        if (itemRespawnCoroutines.ContainsKey(item))
        {
            itemRespawnCoroutines[item] = null;
        }
    }
}
