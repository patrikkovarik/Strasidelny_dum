using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Skript pro kontrolu, zda paprsek prot�n� objekt s dan�m tagem v dan� vzd�lenosti a zda se jm�no prot�naj�c�ho objektu shoduje s ur�en�m jm�nem.
/// </summary>
public class CheckTag : MonoBehaviour
{
    public string tagToCheck = "tag"; // Tag objektu k porovn�n� s prot�naj�c�m objektem
    public float checkDistance = 1.0f; // Vzd�lenost, ve kter� se hled� objekt s dan�m tagem
    public string names = "name"; // Jm�no objektu k porovn�n� s prot�naj�c�m objektem
    public bool isRight = false; // Logick� hodnota, zda se jm�no prot�naj�c�ho objektu shoduje s ur�en�m jm�nem

    void Update()
    {
        // Vytvo�en� paprsku (ray) sm��uj�c�ho nahoru z pozice tohoto objektu
        Ray ray = new Ray(transform.position, Vector3.up);

        // Kontrola zda paprsek (ray) prot�n� n�jak� objekt s dan�m tagem v dan� vzd�lenosti
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, checkDistance))
        {
            // Vykreslen� ��ry od pozice tohoto objektu ke kolizn�mu bodu paprsku
            Debug.DrawLine(transform.position, hit.point, Color.green);

            if (hit.collider.CompareTag(tagToCheck))
            {
                Debug.Log("Objekt " + hit.collider.gameObject.name + " s tagem " + tagToCheck + " se nach�z� nad t�mto objektem.");
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