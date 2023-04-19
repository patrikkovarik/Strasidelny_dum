using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Skript pro kontrolu, zda paprsek protíná objekt s daným tagem v dané vzdálenosti a zda se jméno protínajícího objektu shoduje s urèeným jménem.
/// </summary>
public class CheckTag : MonoBehaviour
{
    public string tagToCheck = "tag"; // Tag objektu k porovnání s protínajícím objektem
    public float checkDistance = 1.0f; // Vzdálenost, ve které se hledá objekt s daným tagem
    public string names = "name"; // Jméno objektu k porovnání s protínajícím objektem
    public bool isRight = false; // Logická hodnota, zda se jméno protínajícího objektu shoduje s urèeným jménem

    void Update()
    {
        // Vytvoøení paprsku (ray) smìøujícího nahoru z pozice tohoto objektu
        Ray ray = new Ray(transform.position, Vector3.up);

        // Kontrola zda paprsek (ray) protíná nìjaký objekt s daným tagem v dané vzdálenosti
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, checkDistance))
        {
            // Vykreslení èáry od pozice tohoto objektu ke koliznímu bodu paprsku
            Debug.DrawLine(transform.position, hit.point, Color.green);

            if (hit.collider.CompareTag(tagToCheck))
            {
                Debug.Log("Objekt " + hit.collider.gameObject.name + " s tagem " + tagToCheck + " se nachází nad tímto objektem.");
            }
            if (names == hit.collider.gameObject.name)
            {
                isRight = true;
                Debug.Log("isRight: " + isRight);
            }
            else
            {
                isRight = false;
                Debug.Log("isRight: " + isRight);
            }

        }
    }
}