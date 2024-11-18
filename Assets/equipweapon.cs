using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public GameObject Gun; // Referenca na orožje
    public Transform WeaponParent; // Referenca na transform za starša orožja

    void Start()
    {
        // Preverimo, ali so objekti dodeljeni
        if (Gun == null || WeaponParent == null)
        {
            Debug.LogError("Gun or WeaponParent is not assigned in the Inspector!");
            return;
        }

        // Poskrbimo, da je orožje kinematično in ga ne premika fizika
        Gun.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        // Če igralec pritisne tipko F, spustimo orožje
        if (Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    void Drop()
    {
        // Prekinemo starševstvo orožja
        WeaponParent.DetachChildren();

        // Popravimo orientacijo orožja
        Gun.transform.eulerAngles = new Vector3(
            Gun.transform.eulerAngles.x,
            Gun.transform.eulerAngles.y,
            Gun.transform.eulerAngles.z
        );

        // Omogočimo fiziko in trke
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        Gun.GetComponent<MeshCollider>().enabled = true;
    }

    void Equip()
    {
        // Onemogočimo fiziko za orožje
        Gun.GetComponent<Rigidbody>().isKinematic = true;

        // Postavimo orožje na pravilno mesto in orientacijo
        Gun.transform.position = WeaponParent.position;
        Gun.transform.rotation = WeaponParent.rotation;

        // Onemogočimo trke
        Gun.GetComponent<MeshCollider>().enabled = false;

        // Pripnemo orožje k staršu
        Gun.transform.SetParent(WeaponParent);
    }

    private void OnTriggerStay(Collider other)
    {
        // Če je igralec v bližini in pritisne tipko E, dvignemo orožje
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Equip();
            }
        }
    }
}
