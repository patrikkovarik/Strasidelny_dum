using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
///  T��da pro Invent�� a pracov�n� s p�edm�ty.
/// </summary>
public class InventoryController : MonoBehaviour
{
    public static List<string> inventoryItems = new List<string>(); // list pro ukl�d�n� polo�ek v invent��i
    public Text inventoryText; // textov� pole pro zobrazen� invent��e
    public GameObject flashlight;
    public GameObject knife;
    public GameObject pistol;
    public GameObject machineGun;

    public void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadInventoryItems();
        }
        else
        {
            Debug.Log("Nov� hra");
            inventoryItems.Clear();
        }
    }


    /// <summary>
    ///  p�id� hr��i �ivot a odeberu polo�ku Heal z invent��e. Pot� aktualizuje textov� pole s invent��em.
    /// </summary>
    public void UseHealItem()
    {
        // p�idat hr��i �ivot a odebrat heal z invent��e
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.maxHealth++; // p�idat hr��i 50 �ivot� (uprav podle pot�eby)
        inventoryItems.Remove("Heal"); // odebrat heal z invent��e
        Debug.Log("�ivoty: " + player.maxHealth);
        // aktualizovat invent��
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
    /// Metoda pro zni�en� p�edm�tu Knife.
    /// </summary>
    public void UseKnife()
    {
        // p�idat hr��i �ivot a odebrat heal z invent��e
        inventoryItems.Remove("Knife"); // odebrat heal z invent��e
        Debug.Log("N�� se zni�il");
        // aktualizovat invent��
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
        // aktualizovat invent��
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
                // Nastavit baterku na neaktivn�
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
                // Nastavit baterku na neaktivn�
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
                // Nastavit baterku na neaktivn�
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
                // Nastavit baterku na neaktivn�
            }
            else
            {
                machineGun.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Metoda pro ulo�en� invent��e.
    /// </summary>
        public void SaveInventoryItems()
        {

            string itemsString = string.Join(",", inventoryItems);

            PlayerPrefs.SetString("inventoryItems", itemsString);

            PlayerPrefs.Save();
        }

    /// <summary>
    /// Metoda pro na�ten� p�edm�t� v invent��i.
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

