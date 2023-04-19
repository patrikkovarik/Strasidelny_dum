using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // klávesa pro interakci s klíèem
    public float interactionDistance = 2f; // maximální vzdálenost, ze které mùže hráè interagovat s klíèem
    private bool isPlayerNearby = false; // indikuje, zda je hráè v blízkosti klíèe
    public GameObject gateFirst;
    public GameObject gateSecond;
    public GameObject finalLevel;
    public GameObject levels;


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
            PlayerPrefs.SetInt("MachineGunDestroyed", 0);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(interactKey) && isPlayerNearby)
        {
            InventoryController.inventoryItems.Add(tag);
            PlayerPrefs.SetInt("MachineGunDestroyed", 1);
            Destroy(gameObject);
            Destroy(gateSecond);
            Destroy(levels);
            gateFirst.SetActive(true);
            finalLevel.SetActive(true);

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
        int destroyed = PlayerPrefs.GetInt("MachineGunDestroyed");

        // Pokud byl objekt znièen, odstranit ho ze scény
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }

}
