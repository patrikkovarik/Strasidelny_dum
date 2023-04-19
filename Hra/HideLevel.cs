using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevel : MonoBehaviour
{
    public GameObject[] levels;
    private bool playerTouchingLevel = false;

    void Start()
    {
        // Zapnut� v�ech level� na za��tku
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

            // Vypnut� v�ech level� krom� tohoto, kter� hr�� pr�v� dot�k�
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

            // Pokud se hr�� nedot�k� ��dn�ho levelu, zapnout v�echny
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
