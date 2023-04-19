using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da FlashlightController ��d� baterku a jej� interakci s hr��em.
/// </summary>
public class FlashlightController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // kl�vesa pro interakci s kl��em
    public float interactionDistance = 2f; // maxim�ln� vzd�lenost, ze kter� m��e hr�� interagovat s kl��em
    private bool isPlayerNearby = false; // indikuje, zda je hr�� v bl�zkosti kl��e


    /// <summary>
    ///  * Inicializace prom�nn�ch na za��tku hry.
    ///  * Pokud hr�� za��n� starou hru, na�te informaci o stavu baterky.
    ///  * V opa�n�m p��pad� nastav� stav baterky na "nezni�ena".
    /// </summary>
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
            PlayerPrefs.SetInt("FlashlightDestroyed", 0);
        }
    }

    /// <summary>
    /// Ovl�d�n� baterky. Pokud hr�� stiskne interak�n� kl�vesu a je pobl� baterky, ulo�� informaci o zni�en� baterky a odstran� baterku ze sc�ny.
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
    /// Zji��uje, zda je hr�� v bl�zkosti baterky.
    /// * P�i ka�d�m pevn�m updatu aktualizuje prom�nnou "isPlayerNearby".
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
    /// Na��t� stav baterky p�i startu star� hry.
    /// Pokud byla baterka zni�ena, odstran� ji ze sc�ny.
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