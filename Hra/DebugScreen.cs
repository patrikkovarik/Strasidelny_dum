using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tento skript slou�� k vytvo�en� obrazovky s informacemi pro v�voj��e.
/// </summary>

public class DebugScreen : MonoBehaviour
{
    private Text text;
    private PlayerMovement player;
    private EnemyAI enemy;
    private EnemyAILevelTwo enemyLevelTwo;
    private EnemyAILevelThree enemyLevelThree;
    private float deltaTime = 0.0f;
    void Start()
    {
        // Z�sk�n� referenc� na Text komponentu, hr��e a nep��tele
        text = GetComponent<Text>();
        player = FindObjectOfType<PlayerMovement>();
        enemy = FindObjectOfType<EnemyAI>();
        enemyLevelTwo = FindObjectOfType<EnemyAILevelTwo>();
        enemyLevelThree = FindObjectOfType<EnemyAILevelThree>();
    }

    void Update()
    {
        // V�po�et FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Sestaven� textu s informacemi
        string debugText = "Hra: Stra�ideln� d�m Autor hry: Patrik Kova��k\n" +
                           "Pozice hr��e: " + GetPosition(player) + "\n" +
                           "Pozice nep��tele: " + GetPosition(enemy) + "\n" +
                           "FPS: " + Mathf.Round(fps) + "\n";

        if (enemyLevelTwo != null)
        {
            debugText = "Hra: Stra�ideln� d�m Autor hry: Patrik Kova��k\n" +
                        "Pozice hr��e: " + GetPosition(player) + "\n" +
                        "Pozice nep��tele: " + GetPosition(enemyLevelTwo) + "\n" +
                        "FPS: " + Mathf.Round(fps) + "\n";
        }
        else if (enemyLevelThree != null)
        {
            debugText = "Hra: Stra�ideln� d�m Autor hry: Patrik Kova��k\n" +
                        "Pozice hr��e: " + GetPosition(player) + "\n" +
                        "Pozice nep��tele: " + GetPosition(enemyLevelThree) + "\n" +
                        "FPS: " + Mathf.Round(fps) + "\n";
        }

        // Vyps�n� textu na obrazovku
        text.text = debugText;
    }

    // Metoda pro z�sk�n� pozice objektu
    Vector3 GetPosition(Component component)
    {
        if (component != null)
        {
            return component.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}