using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída FlashlightController Øídí baterku a její interakci s hráèem.
/// </summary>
public class FlashlightController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // klávesa pro interakci s klíèem
    public float interactionDistance = 2f; // maximální vzdálenost, ze které mùe hráè interagovat s klíèem
    private bool isPlayerNearby = false; // indikuje, zda je hráè v blízkosti klíèe


    /// <summary>
    ///  * Inicializace promìnnıch na zaèátku hry.
    ///  * Pokud hráè zaèíná starou hru, naète informaci o stavu baterky.
    ///  * V opaèném pøípadì nastaví stav baterky na "neznièena".
    /// </summary>
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
            PlayerPrefs.SetInt("FlashlightDestroyed", 0);
        }
    }

    /// <summary>
    /// Ovládání baterky. Pokud hráè stiskne interakèní klávesu a je poblí baterky, uloí informaci o znièení baterky a odstraní baterku ze scény.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(interactKey) && isPlayerNearby)
        {
            InventoryController.inventoryItems.Add(tag);
            PlayerPrefs.SetInt("FlashlightDestroyed", 1);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Zjišuje, zda je hráè v blízkosti baterky.
    /// * Pøi kadém pevném updatu aktualizuje promìnnou "isPlayerNearby".
    /// </summary>
    void FixedUpdate()
    {
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


    /// <summary>
    /// Naèítá stav baterky pøi startu staré hry.
    /// Pokud byla baterka znièena, odstraní ji ze scény.
    /// </summary>
    public void LoadDestroyedStatus()
    {
        int destroyed = PlayerPrefs.GetInt("FlashlightDestroyed");

        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }
}