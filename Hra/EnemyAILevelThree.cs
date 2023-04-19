using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da pro nep��tele. nep��tel se bl�� k hr��i a �to��.
/// </summary>
public class EnemyAILevelThree : MonoBehaviour
{
    public float speed = 2.0f; // rychlost pohybu
    public float attackRange = 2.0f; // vzd�lenost, kdy nep��tel za�to�� na hr��e
    public string groundTag = "Ground"; // tag plochy, po kter� se nep��tel pohybuje
    private Transform player; // reference na hr��e
    private bool isMoving = false; // indikuje, zda se nep��tel pohybuje
    public int numbers = 0;


    void Start()
    {
        // z�sk�me referenci na hr��e
        player = GameObject.FindGameObjectWithTag("Body").transform;

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
        if (!isMoving)
        {
            // zjist�me, zda se nep��tel nach�z� na povrchu s tagem "Ground"
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit))
            {
                if (hit.transform.tag == groundTag)
                {
                    isMoving = true; // pokud je na povrchu, m��e se za��t pohybovat
                }
            }
        }
        else
        {
            // zjist�me vzd�lenost k hr��i
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // pokud jsme dostate�n� bl�zko hr��e, za�to��me na n�j
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                // jinak se pohybujeme sm�rem k hr��i
                transform.LookAt(player);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * speed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Metoda pro v�pis �toku. z d�vodu �k�zky nep�idan� �to�en�.
    /// </summary>
    void Attack()
    {
        Debug.Log("Nep��tel �to�� na hr��e!");
    }

    /// <summary>
    /// Metoda pro ukl�n�n� pozice nep��tele.
    /// </summary>
    public void SavePosition()
    {
        PlayerPrefs.SetFloat(numbers + "EnemyPosX", transform.position.x);
        PlayerPrefs.SetFloat(numbers + "EnemyPosY", transform.position.y);
        PlayerPrefs.SetFloat(numbers + "EnemyPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Metoda na��t�n� pozice nep��tele.
    /// </summary>
    public void LoadPosition()
    {
        Vector3 position = new Vector3(
            PlayerPrefs.GetFloat(numbers + "EnemyPosX"),
            PlayerPrefs.GetFloat(numbers + "EnemyPosY"),
            PlayerPrefs.GetFloat(numbers + "EnemyPosZ")
        );

        transform.position = position;
    }
}
