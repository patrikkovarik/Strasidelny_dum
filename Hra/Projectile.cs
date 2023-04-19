using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // rychlost projektilu
    public int damage = 1; // poškození zpùsobené projektilu

    public Transform target; // cílový objekt (hráè)
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        // Pokud máme cíl, pohybujeme se smìrem k nìmu
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // Pokud nemáme cíl, znièíme projektil
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Pokud narazíme na hráèe, zpùsobíme mu poškození a vytvoøíme nový projektil
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerHealth = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Vytvoøíme novou instanci projektilu na pùvodní pozici a nastavíme jí správný cíl
            GameObject newProjectile = Instantiate(gameObject, startPosition, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.SetTarget(target);
            }

            // Znièíme pùvodní projektil
            Destroy(gameObject);
        }
    }

    // Metoda pro nastavení cíle projektilu
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
