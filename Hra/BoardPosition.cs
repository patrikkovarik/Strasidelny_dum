using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da BoardPosition obsahuje funkce pro ukl�d�n� a na��t�n� pozice a rotace hern� desky v Unity Engine.
/// </summary>
public class BoardPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
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
    /// Ulo�� pozici a rotaci hern� desky do trval� pam�ti.
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
        Debug.Log("Hra byla ulo�ena");
    }

    /// <summary>
    /// Na�te pozici a rotaci hern� desky z trval� pam�ti a nastav� je.
    /// </summary>
    public void LoadGame()
    {
        // Na�ten� pozice hr��e
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
