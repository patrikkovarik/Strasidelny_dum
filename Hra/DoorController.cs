using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro ovládání dveøí.
/// </summary>
public class DoorController : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip soundClip;
    public AudioSource soundSource;

    [Header("Detekce")]
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float radius = 3f;

    [Header("Pozice")]
    public Transform player;
    public Transform endOfDoor;

    [Header("Rotace")]
    public float rotationSpeed = 100f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    [Header("Kontrola")]
    public bool isOpen = false;
    public string keyItem = "key";

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation;
    }

    void Update()
    {
        // Kontrola, zda je hráè v dostateèné blízkosti dveøí.
        if (Vector3.Distance(transform.position, player.position) <= radius)
        {
            // Zkontrolovat, zda je stisknuté tlaèítko myši.
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Zkontrolovat, zda má hráè klíè k otevøení dveøí.
                if (CanOpenDoor())
                {
                    if (isOpen)
                    {
                        CloseDoor();
                    }
                    else
                    {
                        OpenDoor();
                    }
                }
            }
        }
    }

    // Funkce pro kontrolu, zda má hráè klíè k otevøení dveøí.
    bool CanOpenDoor()
    {
        if (InventoryController.inventoryItems.Contains(keyItem))
        {
            return true;
        }
        else
        {
            Debug.Log("Hráè nemá klíè v inventáøi!");
            return false;
        }
    }

    // Funkce pro otevøení dveøí.
    void OpenDoor()
    {
        soundSource.Play();
        transform.RotateAround(endOfDoor.position, Vector3.up, openAngle);
        isOpen = true;
    }

    // Funkce pro zavøení dveøí.
    void CloseDoor()
    {
        soundSource.Play();
        transform.RotateAround(endOfDoor.position, Vector3.up, -openAngle);
        isOpen = false;
    }

    // Funkce pro získání vzdálenosti mezi hráèem a dveømi.
    float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    // Funkce pro nastavení klíèe potøebného k otevøení dveøí.
    void SetKeyItem(string newItem)
    {
        keyItem = newItem;
    }

}
