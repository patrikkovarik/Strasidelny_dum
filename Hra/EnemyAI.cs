using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// EnemyAI tøída pro nepøítele. Enemy získává informace o poloze hráèe.
/// </summary>
public class EnemyAI : MonoBehaviour

{
    [Header("Audio")]
    public AudioClip soundClip;
    public AudioSource soundSource;

    [Header("Pozice")]
    public Transform player;

    [Header("Enemy parametry")]
    public float moveSpeed = 5;
    public float maxDist = 10;
    public float playerHealth;
    public float minDist = 5;
    public float attackCooldown = 1f; // mezièas mezi útoky
    public string names = "";

    [Header("Detekce")]
    public float detectionRadius = 5f; // radius detekce objektù ve scénì
    public LayerMask detectionMask; // vrstva objektù, se kterými se má detekce provádìt
    private bool isPlayerDead = false;
    private float lastAttackTime; // èas posledního útoku
    private PlayerMovement player_a;



    void Start()
    {
        lastAttackTime = -attackCooldown; // nastavení èasu posledního útoku na èas menší než 0, aby byl umožnìn první útok
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
        FindClosestPlayer();
        player_a = FindObjectOfType<PlayerMovement>();
        playerHealth = player_a.maxHealth;
        transform.LookAt(player);

        if (!isPlayerDead && Vector3.Distance(transform.position, player.position) >= minDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            // raycast pro detekci objektù ve scénì
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRadius, detectionMask))
            {
                // pokud se detekuje kolize s objektem, enemy se posune do strany
                Vector3 dir = hit.normal;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, player.position) <= minDist && Time.time - lastAttackTime > attackCooldown)
            {
                soundSource.Play();

                player_a.maxHealth--;
                Debug.Log("životy:" + playerHealth);

                lastAttackTime = Time.time;

                if (playerHealth <= 1)
                {
                    isPlayerDead = true;
                    DetectPlayerDeath();
                }
            }
        }
    }

    /// <summary>
    /// Metoda pro zjištìní smrti a pøechod na scénu Death
    /// </summary>
    public void DetectPlayerDeath()
    {
        if (isPlayerDead)
        {
            Debug.Log("Hráè zemøel.");
            SceneManager.LoadScene("Death");
        }
    }

    /// <summary>
    /// Metoda pro vyhledávání hráèù. Pøipravené pro budoucí multiplayer.
    /// </summary>
    public Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }

    /// <summary>
    /// Metoda pro vidìní hráèe.
    /// </summary>
    public bool CanSeePlayer()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, detectionRadius, detectionMask);
        return isHit && hit.collider.CompareTag("Player");
    }

    /// <summary>
    /// Metoda pro resetování polohy enemy.
    /// </summary>
    public void ResetEnemy()
    {
        isPlayerDead = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Metoda pro získání smìru hráèe
    /// </summary>
    public Vector3 GetPlayerDirection()
    {
        return (player.position - transform.position).normalized;
    }

    /// <summary>
    /// Metoda pro získání vzdálenosti k hráèi.
    /// </summary>
    public float GetDistanceToPlayer()
    {
        if (player != null)
        {
            return Vector3.Distance(transform.position, player.position);
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    /// <summary>
    /// Metoda pro uložení pozice
    /// </summary>
    public void SavePosition()
    {
        PlayerPrefs.SetFloat(names + "EnemyPosX", transform.position.x);
        PlayerPrefs.SetFloat(names + "EnemyPosY", transform.position.y);
        PlayerPrefs.SetFloat(names + "EnemyPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Metoda pro naètení pozice
    /// </summary>
    public void LoadPosition()
    {
        Vector3 position = new Vector3(
            PlayerPrefs.GetFloat(names + "EnemyPosX"),
            PlayerPrefs.GetFloat(names + "EnemyPosY"),
            PlayerPrefs.GetFloat(names + "EnemyPosZ")
        );

        transform.position = position;
    }

}