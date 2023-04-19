using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// Tøída pro øízení pohybu výtahu a umožnìní hráèi nastoupit a vystoupit z výtahu.
/// </summary>
public class ElevatorController : MonoBehaviour
{
    [Header("Pozice")]
    public Transform startPoint; // Poèáteèní pozice výtahu
    public Transform[] endpoints; // Pole s cílovými pozicemi výtahu
    public int currentEndpointIndex = 0; // Index aktuální cílové pozice
    private Vector3 targetPos; // Cílová pozice výtahu


    [Header("Pohyb Výtahu")]
    public float moveSpeed = 5f; // Rychlost pohybu výtahu
    private bool isMoving = false; // Indikuje, zda se výtah pohybuje


    [Header("Pozice hráèe")]
    public float playerDistanceThreshold = 2f; // Maximální vzdálenost hráèe od výtahu, pøi které mùže nastoupit
    public Transform player; // Transform hráèe
    public Rigidbody playerRigidbody; // Rigid body hráèe
    public GameObject Floor0;

    public TagGameLoad TagGameLoad;
    public string names;

    void Start()
    {
        targetPos = startPoint.position; // Nastavení poèáteèní pozice jako cílovou
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadPosition();
        }
        else
        {
            Debug.Log("Nová hra");
        }
    }

    void Update()
    {
        // Kontrola stisknutí tlaèítka pro pohyb výtahu a zda je hráè dostateènì blízko výtahu
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving && Vector3.Distance(transform.position, player.position) < playerDistanceThreshold)
        {
            isMoving = true;
            currentEndpointIndex = (currentEndpointIndex + 1) % endpoints.Length; // Nastavení další cílové pozice
            targetPos = endpoints[currentEndpointIndex].position; // Nastavení cílové pozice
            // Pøidáno zavøení dveøí po vybrání patra
            StartCoroutine(CloseDoor());
        }
        // Pohyb výtahu
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position == targetPos)
            {
                StartCoroutine(OpenDoor()); // Otevøení dveøí
                isMoving = false;
                currentEndpointIndex = (currentEndpointIndex + 1) % endpoints.Length; // Nastavení další cílové pozice
                targetPos = endpoints[currentEndpointIndex].position; // Nastavení cílové pozice
            }
        }

        // Nastavení gravitace hráèe podle pohybu výtahu
        if (transform.position.y > player.position.y)
        {
            playerRigidbody.useGravity = false; 
        }
        else
        {
            playerRigidbody.useGravity = true; 
        }
    }

    IEnumerator CloseDoor()
    {
        // Zavøení dveøí po krátké pauze
        yield return new WaitForSeconds(0f);
        Debug.Log("Dveøe se zavírají.");

        // Získání referencí na dveøe
        GameObject leftDoor = GameObject.Find("LeftDoor");
        GameObject rightDoor = GameObject.Find("RightDoor");

        if (names == "second")
        {
            // Pohyb dveøí do zavøené polohy
            while (leftDoor.transform.localPosition.x > 5.62f && rightDoor.transform.localPosition.x < 2.91f)
            {
                leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, new Vector3(5.62f, 8.42f, -5.06f), Time.deltaTime * 2f);
                rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, new Vector3(2.91f, 8.42f, -5.06f), Time.deltaTime * 2f);
                yield return null;
            }
        }
        else
        {

            while (leftDoor.transform.localPosition.x > 5.87f && rightDoor.transform.localPosition.x < 2.88f)
            {
                leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, new Vector3(5.87f, 8.42f, -5.06f), Time.deltaTime * 2f);
                rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, new Vector3(2.88f, 8.42f, -5.06f), Time.deltaTime * 2f);
                yield return null;
            }

        }
    }

        IEnumerator OpenDoor()
        {
            // Získání referencí na dveøe
            GameObject leftDoor = GameObject.Find("LeftDoor");
            GameObject rightDoor = GameObject.Find("RightDoor");

            // Pohyb dveøí do otevøené polohy
            if (names == "second")
            {
                while (leftDoor.transform.localPosition.x < 8.275434f && rightDoor.transform.localPosition.x > 0.17f)
                {
                    leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, new Vector3(8.275434f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, new Vector3(0.17f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    yield return null;
                }

                // Poèkejte nìkolik sekund, než se dveøe zavøou
                yield return new WaitForSeconds(2f);


            }
            else
            {
                while (leftDoor.transform.localPosition.x < 8.58f && rightDoor.transform.localPosition.x > 0.1f)
                {
                    leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, new Vector3(8.58f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, new Vector3(0.1f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    yield return null;
                }

                // Poèkejte nìkolik sekund, než se dveøe zavøou
                yield return new WaitForSeconds(2f);
            }
        }
    
    public void SetTarget(int index)
    {
        targetPos = endpoints[index].position; // Nastavení cílové pozice
        isMoving = true;
        // Pøidáno zavøení dveøí po vybrání patra
        StartCoroutine(CloseDoor());
    }

    public int GetCurrentFloor()
    {
        return currentEndpointIndex;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void StopElevator()
    {
        isMoving = false;
    }
    public void SavePosition()
    {
        PlayerPrefs.SetInt(names + "CurrentEndpointIndex", currentEndpointIndex); // Uložení aktuálního indexu cílové pozice
        PlayerPrefs.SetFloat(names + "ElevatorPositionX", transform.position.x); // Uložení X souøadnice pozice výtahu
        PlayerPrefs.SetFloat(names + "ElevatorPositionY", transform.position.y); // Uložení Y souøadnice pozice výtahu
        PlayerPrefs.SetFloat(names + "ElevatorPositionZ", transform.position.z); // Uložení Z souøadnice pozice výtahu
    }

    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey(names + "CurrentEndpointIndex")) // Pokud existuje uložený index cílové pozice
        {
            currentEndpointIndex = PlayerPrefs.GetInt(names + "CurrentEndpointIndex"); // Naètení uloženého indexu cílové pozice
        }
        if (PlayerPrefs.HasKey(names + "ElevatorPositionX") && PlayerPrefs.HasKey(names + "ElevatorPositionY") && PlayerPrefs.HasKey(names + "ElevatorPositionZ")) // Pokud jsou uloženy souøadnice pozice výtahu
        {
            float posX = PlayerPrefs.GetFloat(names + "ElevatorPositionX"); // Naètení uložené X souøadnice pozice výtahu
            float posY = PlayerPrefs.GetFloat(names + "ElevatorPositionY"); // Naètení uložené Y souøadnice pozice výtahu
            float posZ = PlayerPrefs.GetFloat(names + "ElevatorPositionZ"); // Naètení uložené Z souøadnice pozice výtahu
            transform.position = new Vector3(posX, posY, posZ); // Nastavení pozice výtahu na naètené souøadnice
        }
    }
}