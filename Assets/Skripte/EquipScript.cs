using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    public Transform PlayerTransform; // Transform roke igralca
    public GameObject Gun; // Objekt puške
    public Camera Camera; // Kamera igralca
    public float range = 2f; // Doseg za zaznavanje objektov
    public float open = 100f; // Neuporabljena vrednost, lahko izbrišeš, če ni potrebna

    private Quaternion originalRotation; // Shranjena prvotna rotacija objekta

    void Start()
    {
        // Shrani začetno rotacijo objekta
        originalRotation = Gun.transform.rotation;

        // Nastavi Rigidbody na kinematično stanje
        Gun.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            if (Gun.transform.parent == PlayerTransform) // Če je trenutno opremljena
            {
                UnequipObject(); // Odloži objekt
            }
            else
            {
                Shoot(); // Preveri in opremi objekt
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        // Uporabi Raycast za preverjanje objektov v dosegu
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                EquipObject(); // Opremi objekt
            }
        }
    }

    void UnequipObject()
    {
        // Odstrani objekt kot otroka igralca
        PlayerTransform.DetachChildren();

        // Ponastavi rotacijo na začetno
        Gun.transform.rotation = originalRotation;

        // Omogoči fizične simulacije
        Gun.GetComponent<Rigidbody>().isKinematic = false;
    }

    void EquipObject()
    {
        // Onemogoči fizične simulacije
        Gun.GetComponent<Rigidbody>().isKinematic = true;

        // Nastavi pozicijo in rotacijo glede na igralca
        Gun.transform.position = PlayerTransform.position;
        Gun.transform.rotation = PlayerTransform.rotation;

        // Nastavi kot otroka igralčevega Transforma
        Gun.transform.SetParent(PlayerTransform);
    }
}
