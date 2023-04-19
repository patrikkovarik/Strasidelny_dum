using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  T��da reprezentuj�c� n��.
/// </summary>
public class Knife : MonoBehaviour
{
    public InventoryController inventory;

    /// <summary>
    ///  Detekce kolize a n�sledn� zni�en� no�e a odstran�n� enemy ze sc�ny.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bodnut�");
        if (collision.gameObject.CompareTag("EnemyAILevelTwo"))
        {
            Debug.Log("Bodnut�");
            Destroy(collision.gameObject); // destroy the enemy object
            inventory.UseKnife();
            gameObject.SetActive(false); // disable the knife game object
        }
    }
}
