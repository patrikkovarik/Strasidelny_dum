using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tento skript slouží k vytvoøení obrazovky s informacemi pro vývojáøe.
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
        // Získání referencí na Text komponentu, hráèe a nepøítele
        text = GetComponent<Text>();
        player = FindObjectOfType<PlayerMovement>();
        enemy = FindObjectOfType<EnemyAI>();
        enemyLevelTwo = FindObjectOfType<EnemyAILevelTwo>();
        enemyLevelThree = FindObjectOfType<EnemyAILevelThree>();
    }

    void Update()
    {
        // Výpoèet FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Sestavení textu s informacemi
        string debugText = "Hra: Strašidelný dùm Autor hry: Patrik Kovaøík\n" +
                           "Pozice hráèe: " + GetPosition(player) + "\n" +
                           "Pozice nepøítele: " + GetPosition(enemy) + "\n" +
                           "FPS: " + Mathf.Round(fps) + "\n";

        if (enemyLevelTwo != null)
        {
            debugText = "Hra: Strašidelný dùm Autor hry: Patrik Kovaøík\n" +
                        "Pozice hráèe: " + GetPosition(player) + "\n" +
                        "Pozice nepøítele: " + GetPosition(enemyLevelTwo) + "\n" +
                        "FPS: " + Mathf.Round(fps) + "\n";
        }
        else if (enemyLevelThree != null)
        {
            debugText = "Hra: Strašidelný dùm Autor hry: Patrik Kovaøík\n" +
                        "Pozice hráèe: " + GetPosition(player) + "\n" +
                        "Pozice nepøítele: " + GetPosition(enemyLevelThree) + "\n" +
                        "FPS: " + Mathf.Round(fps) + "\n";
        }

        // Vypsání textu na obrazovku
        text.text = debugText;
    }

    // Metoda pro získání pozice objektu
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