using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// T��da pro ��zen� pohybu v�tahu a umo�n�n� hr��i nastoupit a vystoupit z v�tahu.
/// </summary>
public class ElevatorController : MonoBehaviour
{
    [Header("Pozice")]
    public Transform startPoint; // Po��te�n� pozice v�tahu
    public Transform[] endpoints; // Pole s c�lov�mi pozicemi v�tahu
    public int currentEndpointIndex = 0; // Index aktu�ln� c�lov� pozice
    private Vector3 targetPos; // C�lov� pozice v�tahu


    [Header("Pohyb V�tahu")]
    public float moveSpeed = 5f; // Rychlost pohybu v�tahu
    private bool isMoving = false; // Indikuje, zda se v�tah pohybuje


    [Header("Pozice hr��e")]
    public float playerDistanceThreshold = 2f; // Maxim�ln� vzd�lenost hr��e od v�tahu, p�i kter� m��e nastoupit
    public Transform player; // Transform hr��e
    public Rigidbody playerRigidbody; // Rigid body hr��e
    public GameObject Floor0;

    public TagGameLoad TagGameLoad;
    public string names;

    void Start()
    {
        targetPos = startPoint.position; // Nastaven� po��te�n� pozice jako c�lovou
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadPosition();
        }
        else
        {
            Debug.Log("Nov� hra");
        }
    }

    void Update()
    {
        // Kontrola stisknut� tla��tka pro pohyb v�tahu a zda je hr�� dostate�n� bl�zko v�tahu
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving && Vector3.Distance(transform.position, player.position) < playerDistanceThreshold)
        {
            isMoving = true;
            currentEndpointIndex = (currentEndpointIndex + 1) % endpoints.Length; // Nastaven� dal�� c�lov� pozice
            targetPos = endpoints[currentEndpointIndex].position; // Nastaven� c�lov� pozice
            // P�id�no zav�en� dve�� po vybr�n� patra
            StartCoroutine(CloseDoor());
        }
        // Pohyb v�tahu
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position == targetPos)
            {
                StartCoroutine(OpenDoor()); // Otev�en� dve��
                isMoving = false;
                currentEndpointIndex = (currentEndpointIndex + 1) % endpoints.Length; // Nastaven� dal�� c�lov� pozice
                targetPos = endpoints[currentEndpointIndex].position; // Nastaven� c�lov� pozice
            }
        }

        // Nastaven� gravitace hr��e podle pohybu v�tahu
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
        // Zav�en� dve�� po kr�tk� pauze
        yield return new WaitForSeconds(0f);
        Debug.Log("Dve�e se zav�raj�.");

        // Z�sk�n� referenc� na dve�e
        GameObject leftDoor = GameObject.Find("LeftDoor");
        GameObject rightDoor = GameObject.Find("RightDoor");

        if (names == "second")
        {
            // Pohyb dve�� do zav�en� polohy
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
            // Z�sk�n� referenc� na dve�e
            GameObject leftDoor = GameObject.Find("LeftDoor");
            GameObject rightDoor = GameObject.Find("RightDoor");

            // Pohyb dve�� do otev�en� polohy
            if (names == "second")
            {
                while (leftDoor.transform.localPosition.x < 8.275434f && rightDoor.transform.localPosition.x > 0.17f)
                {
                    leftDoor.transform.localPosition = Vector3.MoveTowards(leftDoor.transform.localPosition, new Vector3(8.275434f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    rightDoor.transform.localPosition = Vector3.MoveTowards(rightDoor.transform.localPosition, new Vector3(0.17f, 8.42f, -5.06f), Time.deltaTime * 2f);
                    yield return null;
                }

                // Po�kejte n�kolik sekund, ne� se dve�e zav�ou
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

                // Po�kejte n�kolik sekund, ne� se dve�e zav�ou
                yield return new WaitForSeconds(2f);
            }
        }
    
    public void SetTarget(int index)
    {
        targetPos = endpoints[index].position; // Nastaven� c�lov� pozice
        isMoving = true;
        // P�id�no zav�en� dve�� po vybr�n� patra
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
        PlayerPrefs.SetInt(names + "CurrentEndpointIndex", currentEndpointIndex); // Ulo�en� aktu�ln�ho indexu c�lov� pozice
        PlayerPrefs.SetFloat(names + "ElevatorPositionX", transform.position.x); // Ulo�en� X sou�adnice pozice v�tahu
        PlayerPrefs.SetFloat(names + "ElevatorPositionY", transform.position.y); // Ulo�en� Y sou�adnice pozice v�tahu
        PlayerPrefs.SetFloat(names + "ElevatorPositionZ", transform.position.z); // Ulo�en� Z sou�adnice pozice v�tahu
    }

    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey(names + "CurrentEndpointIndex")) // Pokud existuje ulo�en� index c�lov� pozice
        {
            currentEndpointIndex = PlayerPrefs.GetInt(names + "CurrentEndpointIndex"); // Na�ten� ulo�en�ho indexu c�lov� pozice
        }
        if (PlayerPrefs.HasKey(names + "ElevatorPositionX") && PlayerPrefs.HasKey(names + "ElevatorPositionY") && PlayerPrefs.HasKey(names + "ElevatorPositionZ")) // Pokud jsou ulo�eny sou�adnice pozice v�tahu
        {
            float posX = PlayerPrefs.GetFloat(names + "ElevatorPositionX"); // Na�ten� ulo�en� X sou�adnice pozice v�tahu
            float posY = PlayerPrefs.GetFloat(names + "ElevatorPositionY"); // Na�ten� ulo�en� Y sou�adnice pozice v�tahu
            float posZ = PlayerPrefs.GetFloat(names + "ElevatorPositionZ"); // Na�ten� ulo�en� Z sou�adnice pozice v�tahu
            transform.position = new Vector3(posX, posY, posZ); // Nastaven� pozice v�tahu na na�ten� sou�adnice
        }
    }
}