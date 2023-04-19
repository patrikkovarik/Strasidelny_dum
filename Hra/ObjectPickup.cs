using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da pro posouv�n� objekt�.
/// </summary>
public class ObjectPickup : MonoBehaviour
{
    public float throwForce = 10f; // s�la h�zen�
    public string names = "";
    public Transform holdPosition; // pozice, kde se dr�� objekt
    public KeyCode pickupKey = KeyCode.E; // kl�vesa pro zvednut�/odlo�en� objektu
    public float pickupDistance = 2f; // minim�ln� vzd�lenost pro zvednut� objektu
    private bool isHolding = false; // zda je hr�� moment�ln� dr�� objekt
    private Rigidbody objectRigidbody; // Rigidbody objektu, kter� je dr�en hr��em
    private bool isPlayerNearby = false;

    /// <summary>
    /// vykreslen� objektu
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }

    void Start()
    {
        // z�skat Rigidbody komponentu z tohoto objektu
        objectRigidbody = GetComponent<Rigidbody>();
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadObjectPosition();
        }
        else
        {
            Debug.Log("Nov� hra");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (isHolding)
            {
                // hr�� objekt dr��, tak�e ho mus� odlo�it
                DropObject();
            }
            else
            {
                // hr�� objekt nedr��, tak�e ho mus� zvednout
                PickUpObject();
            }
        }

        if (isHolding)
        {
            // nastavit pozici a rotaci objektu, aby byl p�ed hr��em
            Vector3 objectPosition = holdPosition.position + holdPosition.forward;
            objectPosition.y = transform.position.y; // zachovat v��ku hr��e
            objectRigidbody.MovePosition(objectPosition);


            if (Input.GetKeyDown(KeyCode.F))
            {
                // hr�� chce objekt vyhodit, tak�e ho mus� odhodit
                ThrowObject();
            }
        }
    }

    /// <summary>
    /// Metoda pro nalezen� nejbli���ho objektu s tagem "Pickup", kter� je dostate�n� bl�zko hr��i
    /// </summary>
    void PickUpObject()
    {
        // naj�t nejbli��� objekt s tagem "Pickup", kter� je dostate�n� bl�zko hr��i
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        Collider pickupCollider = null;
        float closestDistance = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Pickup"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (isPlayerNearby) // zkontrolovat, zda je objekt dostate�n� bl�zko hr��i
                {
                    closestDistance = distance;
                    pickupCollider = collider;
                }
            }
        }

        if (pickupCollider != null)
        {
            // hr�� na�el objekt, kter� chce zvednout
            isHolding = true;
            objectRigidbody = pickupCollider.GetComponent<Rigidbody>(); // assign the objectRigidbody to the picked up object's Rigidbody component
            objectRigidbody.isKinematic = true; // vypnout fyziku, aby objekt nespadl na zem
        }
    }

    /// <summary>
    /// Metoda pro polo�en� objektu.
    /// </summary>
    void DropObject()
    {
        // hr�� odlo�il objekt, tak�e ho mus� vlo�it zp�t do sc�ny
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
        // hr�� h�z� objekt
        isHolding = false;
        objectRigidbody.isKinematic = true; // zapnout fyziku, aby objekt spadl na zem
        objectRigidbody.AddForce(holdPosition.forward * throwForce, ForceMode.Impulse); // p�idat s�lu h�zen�
    }

    void FixedUpdate()
    {
        // zkontrolovat, zda je hr�� v bl�zkosti kl��e
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
                // hr�� je dostate�n� bl�zko k objektu, aby ho mohl zvednout
                return !IsHoldingObject(); // vr�tit true, pokud hr�� moment�ln� dr�� jin� objekt
            }
        }
        return false; // hr�� nen� dostate�n� bl�zko k ��dn�mu objektu
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
