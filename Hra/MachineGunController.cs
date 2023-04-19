using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // kl�vesa pro interakci s kl��em
    public float interactionDistance = 2f; // maxim�ln� vzd�lenost, ze kter� m��e hr�� interagovat s kl��em
    private bool isPlayerNearby = false; // indikuje, zda je hr�� v bl�zkosti kl��e
    public GameObject gateFirst;
    public GameObject gateSecond;
    public GameObject finalLevel;
    public GameObject levels;


    void Start()
    {

        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadDestroyedStatus();
        }
        else
        {
            Debug.Log("Nov� hra");
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
        // zkontrolovat, zda je hr�� v bl�zkosti kl��e
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
        // Zkontrolovat, zda byl objekt zni�en a ulo�it informaci do prom�nn� destroyed
        int destroyed = PlayerPrefs.GetInt("MachineGunDestroyed");

        // Pokud byl objekt zni�en, odstranit ho ze sc�ny
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }

}
