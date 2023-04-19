using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// T��da reprezentuj�c� chov�n� Bosse.
/// </summary>

public class BossAI : MonoBehaviour
{
    private Transform target;
    public float movementSpeed = 3f; // rychlost pohybu bosse
    public float rotationSpeed = 3f; // rychlost rotace bosse
    public float attackRange = 10f; // dosah �toku bosse
    public float attackRate = 2f; // rychlost �toku bosse
    private float nextAttackTime = 0f; // �as dal��ho �toku bosse
    public GameObject projectile; // prefab projektilu
    public Transform firePoint; // pozice pro vyst�elen� projektilu
    public int health = 100; // zdrav� bosse
    public GameObject winScreen; // obrazovka pro v�t�zstv�
    private bool isPlayerInRange = false; // indikuje, zda je hr�� v dosahu �toku bosse
    private PlayerMovement player; // reference na t��du hr��e
    public float playerHealth; // zdrav� hr��e
    private bool isPlayerDead = false; // indikuje, zda je hr�� mrtv�
    void Start()
    {
        // Nastaven� c�le AI na hr��e
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Z�sk�n� informac� o hr��i
        player = FindObjectOfType<PlayerMovement>();
        playerHealth = player.maxHealth;
        Debug.Log(playerHealth);
        // Detekce, zda je hr�� v dosahu �toku bosse a pokud ano, proveden� �toku
        isPlayerInRange = IsPlayerInRange();
        if (isPlayerInRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
            Attack();
        }
        // Detekce smrti hr��e
        if (playerHealth == 0)
        {
            isPlayerDead = true;
            DetectPlayerDeath();
        }
    }

    // Metoda pro zji�t�n�, zda je hr�� v dosahu �toku bosse
    private bool IsPlayerInRange()
    {
        if (target == null) return false; // Pokud nen� target nastaven, hr�� nen� v dosahu
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        return distanceToPlayer <= attackRange;
    }

    // Metoda pro �tok na hr��e
    void Attack()
    {
        // Zkontrolovat, zda je hr�� v dosahu a pokud ano, tak vytvo�it projektil a vyst�elit ho sm�rem k hr��i
        if (isPlayerInRange)
        {
            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.target = target;
        }
    }

    // Metoda pro odebr�n� zdrav� bossovi
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Debug.Log("Nep��tel zem�el");
            Die();
        }
    }

    // Metoda pro smrt bosse
    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Win", LoadSceneMode.Single);
    }
    // Metoda pro smrt hr��e
    public void DetectPlayerDeath()
    {
        if (isPlayerDead)
        {
            Debug.Log("Hr�� zem�el.");
            SceneManager.LoadScene("Death");
        }
    }




}
