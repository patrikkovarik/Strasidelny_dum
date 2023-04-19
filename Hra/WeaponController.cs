using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // klávesa pro interakci s klíèem
    public float interactionDistance = 2f; // maximální vzdálenost, ze které mùže hráè interagovat s klíèem
    private bool isPlayerNearby = false; // indikuje, zda je hráè v blízkosti klíèe
    public GameObject[] activateOnDestroy; // pole objektù, které se aktivují po znièení zbranì



    void Start()
    {

        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadDestroyedStatus();
        }
        else
        {
            Debug.Log("Nová hra");
            PlayerPrefs.SetInt("PistolDestroyed", 0);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(interactKey) && isPlayerNearby)
        {
            InventoryController.inventoryItems.Add(tag);
            PlayerPrefs.SetInt("PistolDestroyed", 1);
            Destroy(gameObject);
            foreach (GameObject obj in activateOnDestroy) // pro každý objekt v poli
            {
                obj.SetActive(true); // aktivovat objekt
            }
        }


    }

    void FixedUpdate()
    {
        // zkontrolovat, zda je hráè v blízkosti klíèe
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < interactionDistance)
        {
            isPlayerNearby = true;
        }
        else
        {
            isPlayerNearby = false;
        }
    }

    public void LoadDestroyedStatus()
    {
        // Zkontrolovat, zda byl objekt znièen a uložit informaci do promìnné destroyed
        int destroyed = PlayerPrefs.GetInt("PistolDestroyed");

        // Pokud byl objekt znièen, odstranit ho ze scény
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }

}
