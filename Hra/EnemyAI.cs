using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// EnemyAI t��da pro nep��tele. Enemy z�sk�v� informace o poloze hr��e.
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
    public float attackCooldown = 1f; // mezi�as mezi �toky
    public string names = "";

    [Header("Detekce")]
    public float detectionRadius = 5f; // radius detekce objekt� ve sc�n�
    public LayerMask detectionMask; // vrstva objekt�, se kter�mi se m� detekce prov�d�t
    private bool isPlayerDead = false;
    private float lastAttackTime; // �as posledn�ho �toku
    private PlayerMovement player_a;



    void Start()
    {
        lastAttackTime = -attackCooldown; // nastaven� �asu posledn�ho �toku na �as men�� ne� 0, aby byl umo�n�n prvn� �tok
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
        FindClosestPlayer();
        player_a = FindObjectOfType<PlayerMovement>();
        playerHealth = player_a.maxHealth;
        transform.LookAt(player);

        if (!isPlayerDead && Vector3.Distance(transform.position, player.position) >= minDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            // raycast pro detekci objekt� ve sc�n�
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
                Debug.Log("�ivoty:" + playerHealth);

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
    /// Metoda pro zji�t�n� smrti a p�echod na sc�nu Death
    /// </summary>
    public void DetectPlayerDeath()
    {
        if (isPlayerDead)
        {
            Debug.Log("Hr�� zem�el.");
            SceneManager.LoadScene("Death");
        }
    }

    /// <summary>
    /// Metoda pro vyhled�v�n� hr���. P�ipraven� pro budouc� multiplayer.
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
    /// Metoda pro vid�n� hr��e.
    /// </summary>
    public bool CanSeePlayer()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, detectionRadius, detectionMask);
        return isHit && hit.collider.CompareTag("Player");
    }

    /// <summary>
    /// Metoda pro resetov�n� polohy enemy.
    /// </summary>
    public void ResetEnemy()
    {
        isPlayerDead = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Metoda pro z�sk�n� sm�ru hr��e
    /// </summary>
    public Vector3 GetPlayerDirection()
    {
        return (player.position - transform.position).normalized;
    }

    /// <summary>
    /// Metoda pro z�sk�n� vzd�lenosti k hr��i.
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
    /// Metoda pro ulo�en� pozice
    /// </summary>
    public void SavePosition()
    {
        PlayerPrefs.SetFloat(names + "EnemyPosX", transform.position.x);
        PlayerPrefs.SetFloat(names + "EnemyPosY", transform.position.y);
        PlayerPrefs.SetFloat(names + "EnemyPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Metoda pro na�ten� pozice
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