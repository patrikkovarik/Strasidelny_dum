using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da pro pro odstran�n� p�edm�tu ze sc�ny a p�id�n� do invent��e.
/// </summary>
public class HealItem : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0; // kl�vesa pro interakci s kl��em
    public float interactionDistance = 2f; // maxim�ln� vzd�lenost, ze kter� m��e hr�� interagovat s kl��em
    private bool isPlayerNearby = false; // indikuje, zda je hr�� v bl�zkosti kl��e
    public string names = "";

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
    /// Metoda kontroluje zda je v bl�zkosti Itemu.
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
    /// Metoda pro na�ten� jestli je objekt zni�en�
    /// </summary>
    public void LoadDestroyedStatus()
    {
        // Zkontrolovat, zda byl objekt zni�en a ulo�it informaci do prom�nn� destroyed
        int destroyed = PlayerPrefs.GetInt(names + "_destroyed");

        // Pokud byl objekt zni�en, odstranit ho ze sc�ny
        if (destroyed == 1)
        {
            Destroy(gameObject);
        }
    }
}