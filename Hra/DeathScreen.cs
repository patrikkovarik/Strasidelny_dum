using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Zobraz� obrazovku smrti s odpov�daj�c� zpr�vou a spust� na��t�n� dal�� sc�ny.
/// </summary>
public class DeathScreen : MonoBehaviour
{
    public Text deathText; // Text deathText


    public void ShowDeathScreen()
    {
        deathText.text = "jsi mrtv�!\n zkus to znovu";
        gameObject.SetActive(true); // aktivov�n� deathScreen

        StartCoroutine(LoadNextScene());
    }

    /// <summary>
    /// Metoda pro na�ten� sc�ny po �ase
    /// </summary>
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("SampleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowDeathScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
