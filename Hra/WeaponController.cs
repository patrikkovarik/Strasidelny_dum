using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // kl�vesa pro interakci s kl��em
    public float interactionDistance = 2f; // maxim�ln� vzd�lenost, ze kter� m��e hr�� interagovat s kl��em
    private bool isPlayerNearby = false; // indikuje, zda je hr�� v bl�zkosti kl��e
    public GameObject[] activateOnDestroy; // pole objekt�, kter� se aktivuj� po zni�en� zbran�



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
            foreach (GameObject obj in activateOnDestroy) // pro ka�d� objekt v poli
            {
                obj.SetActive(true); // aktivovat objekt
            }
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
        int destroyed = PlayerPrefs.GetInt("PistolDestroyed");

        // Pokud byl objekt zni�en, odstranit ho ze sc�ny
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }

}
