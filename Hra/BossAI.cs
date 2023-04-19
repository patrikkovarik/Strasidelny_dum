using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tøída reprezentující chování Bosse.
/// </summary>

public class BossAI : MonoBehaviour
{
    private Transform target;
    public float movementSpeed = 3f; // rychlost pohybu bosse
    public float rotationSpeed = 3f; // rychlost rotace bosse
    public float attackRange = 10f; // dosah útoku bosse
    public float attackRate = 2f; // rychlost útoku bosse
    private float nextAttackTime = 0f; // èas dalšího útoku bosse
    public GameObject projectile; // prefab projektilu
    public Transform firePoint; // pozice pro vystøelení projektilu
    public int health = 100; // zdraví bosse
    public GameObject winScreen; // obrazovka pro vítìzství
    private bool isPlayerInRange = false; // indikuje, zda je hráè v dosahu útoku bosse
    private PlayerMovement player; // reference na tøídu hráèe
    public float playerHealth; // zdraví hráèe
    private bool isPlayerDead = false; // indikuje, zda je hráè mrtvý
    void Start()
    {
        // Nastavení cíle AI na hráèe
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Získání informací o hráèi
        player = FindObjectOfType<PlayerMovement>();
        playerHealth = player.maxHealth;
        Debug.Log(playerHealth);
        // Detekce, zda je hráè v dosahu útoku bosse a pokud ano, provedení útoku
        isPlayerInRange = IsPlayerInRange();
        if (isPlayerInRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
            Attack();
        }
        // Detekce smrti hráèe
        if (playerHealth == 0)
        {
            isPlayerDead = true;
            DetectPlayerDeath();
        }
    }

    // Metoda pro zjištìní, zda je hráè v dosahu útoku bosse
    private bool IsPlayerInRange()
    {
        if (target == null) return false; // Pokud není target nastaven, hráè není v dosahu
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        return distanceToPlayer <= attackRange;
    }

    // Metoda pro útok na hráèe
    void Attack()
    {
        // Zkontrolovat, zda je hráè v dosahu a pokud ano, tak vytvoøit projektil a vystøelit ho smìrem k hráèi
        if (isPlayerInRange)
        {
            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.target = target;
        }
    }

    // Metoda pro odebrání zdraví bossovi
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Debug.Log("Nepøítel zemøel");
            Die();
        }
    }

    // Metoda pro smrt bosse
    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Win", LoadSceneMode.Single);
    }
    // Metoda pro smrt hráèe
    public void DetectPlayerDeath()
    {
        if (isPlayerDead)
        {
            Debug.Log("Hráè zemøel.");
            SceneManager.LoadScene("Death");
        }
    }




}
