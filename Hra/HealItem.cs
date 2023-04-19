using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro pro odstranìní pøedmìtu ze scény a pøidání do inventáøe.
/// </summary>
public class HealItem : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // klávesa pro interakci s klíèem
    public float interactionDistance = 2f; // maximální vzdálenost, ze které mùže hráè interagovat s klíèem
    private bool isPlayerNearby = false; // indikuje, zda je hráè v blízkosti klíèe
    public string names = "";

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
            PlayerPrefs.SetInt(names + "_destroyed", 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && isPlayerNearby)
        {
            InventoryController.inventoryItems.Add(tag);
            PlayerPrefs.SetInt(names + "_destroyed", 1);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Metoda kontroluje zda je v blízkosti Itemu.
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
    /// Metoda pro naètení jestli je objekt znièený
    /// </summary>
    public void LoadDestroyedStatus()
    {
        // Zkontrolovat, zda byl objekt znièen a uložit informaci do promìnné destroyed
        int destroyed = PlayerPrefs.GetInt(names + "_destroyed");

        // Pokud byl objekt znièen, odstranit ho ze scény
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }
}