using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da pro ovl�d�n� dve��.
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
        // Kontrola, zda je hr�� v dostate�n� bl�zkosti dve��.
        if (Vector3.Distance(transform.position, player.position) <= radius)
        {
            // Zkontrolovat, zda je stisknut� tla��tko my�i.
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Zkontrolovat, zda m� hr�� kl�� k otev�en� dve��.
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

    // Funkce pro kontrolu, zda m� hr�� kl�� k otev�en� dve��.
    bool CanOpenDoor()
    {
        if (InventoryController.inventoryItems.Contains(keyItem))
        {
            return true;
        }
        else
        {
            Debug.Log("Hr�� nem� kl�� v invent��i!");
            return false;
        }
    }

    // Funkce pro otev�en� dve��.
    void OpenDoor()
    {
        soundSource.Play();
        transform.RotateAround(endOfDoor.position, Vector3.up, openAngle);
        isOpen = true;
    }

    // Funkce pro zav�en� dve��.
    void CloseDoor()
    {
        soundSource.Play();
        transform.RotateAround(endOfDoor.position, Vector3.up, -openAngle);
        isOpen = false;
    }

    // Funkce pro z�sk�n� vzd�lenosti mezi hr��em a dve�mi.
    float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    // Funkce pro nastaven� kl��e pot�ebn�ho k otev�en� dve��.
    void SetKeyItem(string newItem)
    {
        keyItem = newItem;
    }

}
