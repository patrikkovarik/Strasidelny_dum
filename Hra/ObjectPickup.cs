using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro posouvání objektù.
/// </summary>
public class ObjectPickup : MonoBehaviour
{
    public float throwForce = 10f; // síla házení
    public string names = "";
    public Transform holdPosition; // pozice, kde se drží objekt
    public KeyCode pickupKey = KeyCode.E; // klávesa pro zvednutí/odložení objektu
    public float pickupDistance = 2f; // minimální vzdálenost pro zvednutí objektu
    private bool isHolding = false; // zda je hráè momentálnì drží objekt
    private Rigidbody objectRigidbody; // Rigidbody objektu, který je držen hráèem
    private bool isPlayerNearby = false;

    /// <summary>
    /// vykreslení objektu
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }

    void Start()
    {
        // získat Rigidbody komponentu z tohoto objektu
        objectRigidbody = GetComponent<Rigidbody>();
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadObjectPosition();
        }
        else
        {
            Debug.Log("Nová hra");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (isHolding)
            {
                // hráè objekt drží, takže ho musí odložit
                DropObject();
            }
            else
            {
                // hráè objekt nedrží, takže ho musí zvednout
                PickUpObject();
            }
        }

        if (isHolding)
        {
            // nastavit pozici a rotaci objektu, aby byl pøed hráèem
            Vector3 objectPosition = holdPosition.position + holdPosition.forward;
            objectPosition.y = transform.position.y; // zachovat výšku hráèe
            objectRigidbody.MovePosition(objectPosition);


            if (Input.GetKeyDown(KeyCode.F))
            {
                // hráè chce objekt vyhodit, takže ho musí odhodit
                ThrowObject();
            }
        }
    }

    /// <summary>
    /// Metoda pro nalezení nejbližšího objektu s tagem "Pickup", který je dostateènì blízko hráèi
    /// </summary>
    void PickUpObject()
    {
        // najít nejbližší objekt s tagem "Pickup", který je dostateènì blízko hráèi
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        Collider pickupCollider = null;
        float closestDistance = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Pickup"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (isPlayerNearby) // zkontrolovat, zda je objekt dostateènì blízko hráèi
                {
                    closestDistance = distance;
                    pickupCollider = collider;
                }
            }
        }

        if (pickupCollider != null)
        {
            // hráè našel objekt, který chce zvednout
            isHolding = true;
            objectRigidbody = pickupCollider.GetComponent<Rigidbody>(); // assign the objectRigidbody to the picked up object's Rigidbody component
            objectRigidbody.isKinematic = true; // vypnout fyziku, aby objekt nespadl na zem
        }
    }

    /// <summary>
    /// Metoda pro položení objektu.
    /// </summary>
    void DropObject()
    {
        // hráè odložil objekt, takže ho musí vložit zpìt do scény
        isHolding = false;
        objectRigidbody.isKinematic = true; // zapnout fyziku, aby objekt spadl na zem
        objectRigidbody = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Pickup") && !collider.gameObject.activeSelf)
            {
                collider.gameObject.SetActive(true);
                break;
            }
        }
    }
    void ThrowObject()
    {
        // hráè hází objekt
        isHolding = false;
        objectRigidbody.isKinematic = true; // zapnout fyziku, aby objekt spadl na zem
        objectRigidbody.AddForce(holdPosition.forward * throwForce, ForceMode.Impulse); // pøidat sílu házení
    }

    void FixedUpdate()
    {
        // zkontrolovat, zda je hráè v blízkosti klíèe
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < pickupDistance)
        {
            isPlayerNearby = true;
        }
        else
        {
            isPlayerNearby = false;
        }
    }
    bool IsHoldingObject()
    {
        return isHolding;
    }

    bool IsObjectInRange(GameObject obj)
    {
        float distance = Vector3.Distance(transform.position, obj.transform.position);
        return distance <= pickupDistance;
    }

    bool CanPickUpObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Pickup") && IsObjectInRange(collider.gameObject))
            {
                // hráè je dostateènì blízko k objektu, aby ho mohl zvednout
                return !IsHoldingObject(); // vrátit true, pokud hráè momentálnì drží jiný objekt
            }
        }
        return false; // hráè není dostateènì blízko k žádnému objektu
    }

    bool IsObjectHeldByPlayer(GameObject obj)
    {
        return obj != null && obj == objectRigidbody.gameObject;
    }

    public void SaveObjectPosition()
    {
        PlayerPrefs.SetFloat(names + "ObjectPosX", transform.position.x);
        PlayerPrefs.SetFloat(names + "ObjectPosY", transform.position.y);
        PlayerPrefs.SetFloat(names + "ObjectPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    public void LoadObjectPosition()
    {
        float x = PlayerPrefs.GetFloat(names + "ObjectPosX");
        float y = PlayerPrefs.GetFloat(names + "ObjectPosY");
        float z = PlayerPrefs.GetFloat(names + "ObjectPosZ");
        transform.position = new Vector3(x, y, z);
    }


}
