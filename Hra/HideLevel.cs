using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevel : MonoBehaviour
{
    public GameObject[] levels;
    private bool playerTouchingLevel = false;

    void Start()
    {
        // Zapnutí všech levelù na zaèátku
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(true);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTouchingLevel = true;

            // Vypnutí všech levelù kromì tohoto, který hráè právì dotýká
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] == gameObject)
                {
                    levels[i].SetActive(true);
                }
                else
                {
                    levels[i].SetActive(false);
                }
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTouchingLevel = false;

            // Pokud se hráè nedotýká žádného levelu, zapnout všechny
            if (!playerTouchingLevel)
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    levels[i].SetActive(true);
                }
            }
        }
    }
}
