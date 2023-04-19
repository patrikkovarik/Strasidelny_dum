using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro pohyb blokù.
/// </summary>
public class MapBlock : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 5f;
    public float minDistance = 0.1f;
    private Vector3 currentTarget;

    void Start()
    {
        currentTarget = startPoint.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, currentTarget);

        if (distance < minDistance)
        {
            if (currentTarget == startPoint.position)
            {
                currentTarget = endPoint.position;
            }
            else
            {
                currentTarget = startPoint.position;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    /// <summary>
    /// Metoda pro detekci hráèe na objektu
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hráè je na objektu");

        }
    }




}
