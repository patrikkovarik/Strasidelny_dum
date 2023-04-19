using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro nepøítele. nepøítel se blíží k hráèi a útoèí.
/// </summary>
public class EnemyAILevelThree : MonoBehaviour
{
    public float speed = 2.0f; // rychlost pohybu
    public float attackRange = 2.0f; // vzdálenost, kdy nepøítel zaútoèí na hráèe
    public string groundTag = "Ground"; // tag plochy, po které se nepøítel pohybuje
    private Transform player; // reference na hráèe
    private bool isMoving = false; // indikuje, zda se nepøítel pohybuje
    public int numbers = 0;


    void Start()
    {
        // získáme referenci na hráèe
        player = GameObject.FindGameObjectWithTag("Body").transform;

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
        if (!isMoving)
        {
            // zjistíme, zda se nepøítel nachází na povrchu s tagem "Ground"
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit))
            {
                if (hit.transform.tag == groundTag)
                {
                    isMoving = true; // pokud je na povrchu, mùže se zaèít pohybovat
                }
            }
        }
        else
        {
            // zjistíme vzdálenost k hráèi
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // pokud jsme dostateènì blízko hráèe, zaútoèíme na nìj
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                // jinak se pohybujeme smìrem k hráèi
                transform.LookAt(player);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * speed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Metoda pro výpis útoku. z dùvodu úkázky nepøidané útoèení.
    /// </summary>
    void Attack()
    {
        Debug.Log("Nepøítel útoèí na hráèe!");
    }

    /// <summary>
    /// Metoda pro uklánání pozice nepøítele.
    /// </summary>
    public void SavePosition()
    {
        PlayerPrefs.SetFloat(numbers + "EnemyPosX", transform.position.x);
        PlayerPrefs.SetFloat(numbers + "EnemyPosY", transform.position.y);
        PlayerPrefs.SetFloat(numbers + "EnemyPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Metoda naèítání pozice nepøítele.
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
