using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída pro správu pozice stolu v herním svìtì. zatím nepøidán a nedokonèen.
/// </summary>
public class DeskPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType); // zobrazení typu hry v textovém prvku
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
    /// Uloží pozici stolu do uložištì.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("DeskPositionX", transform.position.x);
        PlayerPrefs.SetFloat("DeskPositionY", transform.position.y);
        PlayerPrefs.SetFloat("DeskPositionZ", transform.position.z);

        PlayerPrefs.Save();
        Debug.Log("Hra byla uložena");
    }

    /// <summary>
    /// Naète pozici stolu z uložištì.
    /// </summary>
    public void LoadGame()
    {
        // Naètení pozice hráèe
        float x = PlayerPrefs.GetFloat("DeskPositionX");
        float y = PlayerPrefs.GetFloat("DeskPositionY");
        float z = PlayerPrefs.GetFloat("DeskPositionZ");
        transform.position = new Vector3(x, y, z);
    }
}