using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // rychlost projektilu
    public int damage = 1; // po�kozen� zp�soben� projektilu

    public Transform target; // c�lov� objekt (hr��)
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        // Pokud m�me c�l, pohybujeme se sm�rem k n�mu
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // Pokud nem�me c�l, zni��me projektil
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Pokud naraz�me na hr��e, zp�sob�me mu po�kozen� a vytvo��me nov� projektil
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerHealth = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Vytvo��me novou instanci projektilu na p�vodn� pozici a nastav�me j� spr�vn� c�l
            GameObject newProjectile = Instantiate(gameObject, startPosition, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.SetTarget(target);
            }

            // Zni��me p�vodn� projektil
            Destroy(gameObject);
        }
    }

    // Metoda pro nastaven� c�le projektilu
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
