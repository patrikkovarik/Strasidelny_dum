using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da pro spr�vu pozice stolu v hern�m sv�t�. zat�m nep�id�n a nedokon�en.
/// </summary>
public class DeskPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType); // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadGame();
        }
        else
        {
            Debug.Log("Nov� hra");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Ulo�� pozici stolu do ulo�i�t�.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("DeskPositionX", transform.position.x);
        PlayerPrefs.SetFloat("DeskPositionY", transform.position.y);
        PlayerPrefs.SetFloat("DeskPositionZ", transform.position.z);

        PlayerPrefs.Save();
        Debug.Log("Hra byla ulo�ena");
    }

    /// <summary>
    /// Na�te pozici stolu z ulo�i�t�.
    /// </summary>
    public void LoadGame()
    {
        // Na�ten� pozice hr��e
        float x = PlayerPrefs.GetFloat("DeskPositionX");
        float y = PlayerPrefs.GetFloat("DeskPositionY");
        float z = PlayerPrefs.GetFloat("DeskPositionZ");
        transform.position = new Vector3(x, y, z);
    }
}