using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
///  Tøída pro Inventáø a pracování s pøedmìty.
/// </summary>
public class InventoryController : MonoBehaviour
{
    public static List<string> inventoryItems = new List<string>(); // list pro ukládání položek v inventáøi
    public Text inventoryText; // textové pole pro zobrazení inventáøe
    public GameObject flashlight;
    public GameObject knife;
    public GameObject pistol;
    public GameObject machineGun;

    public void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadInventoryItems();
        }
        else
        {
            Debug.Log("Nová hra");
            inventoryItems.Clear();
        }
    }


    /// <summary>
    ///  pøidá hráèi život a odeberu položku Heal z inventáøe. Poté aktualizuje textové pole s inventáøem.
    /// </summary>
    public void UseHealItem()
    {
        // pøidat hráèi život a odebrat heal z inventáøe
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.maxHealth++; // pøidat hráèi 50 životù (uprav podle potøeby)
        inventoryItems.Remove("Heal"); // odebrat heal z inventáøe
        Debug.Log("životy: " + player.maxHealth);
        // aktualizovat inventáø
        if (inventoryItems.Count > 0)
        {
            inventoryText.text = string.Join(", ", inventoryItems);
        }
        else
        {
            inventoryText.text = "";
        }
    }


    /// <summary>
    /// Metoda pro znièení pøedmìtu Knife.
    /// </summary>
    public void UseKnife()
    {
        // pøidat hráèi život a odebrat heal z inventáøe
        inventoryItems.Remove("Knife"); // odebrat heal z inventáøe
        Debug.Log("Nùž se znièil");
        // aktualizovat inventáø
        if (inventoryItems.Count > 0)
        {
            inventoryText.text = string.Join(", ", inventoryItems);
        }
        else
        {
            inventoryText.text = "";
        }
    }

    void Update()
    {
        // aktualizovat inventáø
        if (inventoryItems.Count > 0)
        {
            inventoryText.text = string.Join(", ", inventoryItems);
        }
        else
        {
            inventoryText.text = "";
        }

        if (Input.GetKeyDown(KeyCode.H) && inventoryItems.Contains("Heal"))
        {
            UseHealItem();
        }

        if (Input.GetKeyDown(KeyCode.G) && inventoryItems.Contains("Flashlight"))
        {
            // Disable all other items
            knife.SetActive(false);
            pistol.SetActive(false);
            machineGun.SetActive(false);

            if (flashlight.activeSelf)
            {
                flashlight.SetActive(false);
                // Nastavit baterku na neaktivní
            }
            else
            {
                flashlight.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.T) && inventoryItems.Contains("Knife"))
        {
            // Disable all other items
            flashlight.SetActive(false);
            pistol.SetActive(false);
            machineGun.SetActive(false);

            if (knife.activeSelf)
            {
                knife.SetActive(false);
                // Nastavit baterku na neaktivní
            }
            else
            {
                knife.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryItems.Contains("Pistol"))
        {
            // Disable all other items
            flashlight.SetActive(false);
            knife.SetActive(false);
            machineGun.SetActive(false);

            if (pistol.activeSelf)
            {
                pistol.SetActive(false);
                // Nastavit baterku na neaktivní
            }
            else
            {
                pistol.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryItems.Contains("MachineGun"))
        {
            flashlight.SetActive(false);
            knife.SetActive(false);
            pistol.SetActive(false);

            if (machineGun.activeSelf)
            {
                machineGun.SetActive(false);
                // Nastavit baterku na neaktivní
            }
            else
            {
                machineGun.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Metoda pro uložení inventáøe.
    /// </summary>
        public void SaveInventoryItems()
        {

            string itemsString = string.Join(",", inventoryItems);

            PlayerPrefs.SetString("inventoryItems", itemsString);

            PlayerPrefs.Save();
        }

    /// <summary>
    /// Metoda pro naètení pøedmìtù v inventáøi.
    /// </summary>
    /// <returns>Inventory pole</returns>
        public List<string> LoadInventoryItems()
        {
            string itemsString = PlayerPrefs.GetString("inventoryItems", "");

            if (string.IsNullOrEmpty(itemsString))
            {
                return new List<string>();
            }

            string[] itemsArray = itemsString.Split(',');

            return new List<string>(itemsArray);
        }

    }

