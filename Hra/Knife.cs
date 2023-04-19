using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Tøída reprezentující nùž.
/// </summary>
public class Knife : MonoBehaviour
{
    public InventoryController inventory;

    /// <summary>
    ///  Detekce kolize a následné znièení nože a odstranìní enemy ze scény.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bodnutí");
        if (collision.gameObject.CompareTag("EnemyAILevelTwo"))
        {
            Debug.Log("Bodnutí");
            Destroy(collision.gameObject); // destroy the enemy object
            inventory.UseKnife();
            gameObject.SetActive(false); // disable the knife game object
        }
    }
}
