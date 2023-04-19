using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída BoardPosition obsahuje funkce pro ukládání a naèítání pozice a rotace herní desky v Unity Engine.
/// </summary>
public class BoardPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadGame();
        }
        else
        {
            Debug.Log("Nová hra");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Uloží pozici a rotaci herní desky do trvalé pamìti.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("BoardPositionX", transform.position.x);
        PlayerPrefs.SetFloat("BoardPositionY", transform.position.y);
        PlayerPrefs.SetFloat("BoardPositionZ", transform.position.z);

        PlayerPrefs.SetFloat("BoardRotationX", transform.rotation.x);
        PlayerPrefs.SetFloat("BoardRotationY", transform.rotation.y);
        PlayerPrefs.SetFloat("BoardRotationZ", transform.rotation.z);

        PlayerPrefs.Save();
        Debug.Log("Hra byla uložena");
    }

    /// <summary>
    /// Naète pozici a rotaci herní desky z trvalé pamìti a nastaví je.
    /// </summary>
    public void LoadGame()
    {
        // Naètení pozice hráèe
        float x = PlayerPrefs.GetFloat("BoardPositionX");
        float y = PlayerPrefs.GetFloat("BoardPositionY");
        float z = PlayerPrefs.GetFloat("BoardPositionZ");
        transform.position = new Vector3(x, y, z);

        float rx = PlayerPrefs.GetFloat("BoardRotationX");
        float ry = PlayerPrefs.GetFloat("BoardRotationY");
        float rz = PlayerPrefs.GetFloat("BoardRotationZ");
        transform.rotation = Quaternion.Euler(rx, ry, rz);
    }
}
