using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Zobrazí obrazovku smrti s odpovídající zprávou a spustí naèítání další scény.
/// </summary>
public class DeathScreen : MonoBehaviour
{
    public Text deathText; // Text deathText


    public void ShowDeathScreen()
    {
        deathText.text = "jsi mrtvý!\n zkus to znovu";
        gameObject.SetActive(true); // aktivování deathScreen

        StartCoroutine(LoadNextScene());
    }

    /// <summary>
    /// Metoda pro naètení scény po èase
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
