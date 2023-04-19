using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagGameLoad : MonoBehaviour
{
    private const string PREFS_KEY = "gameType"; // konstanta pro uložení klíèe PlayerPrefs
    private bool oldGame = false;
    private bool newGame = false;

    void Start()
    {
        // naètení hodnoty uložené v PlayerPrefs pøi spuštìní scény
        LoadGameType();
    }

    public void GetTagName()
    {
        if (tag == "OldGame")
        {
            oldGame = true;
            newGame = false;

            SaveGameType(); // uložení hodnoty "OldGame" v PlayerPrefs
        }
        else if (tag == "NewGame")
        {
            newGame = true;
            oldGame = false;

            SaveGameType(); // uložení hodnoty "NewGame" v PlayerPrefs
        }
    }

    public void LoadGameType()
    {
        // naètení hodnoty uložené v PlayerPrefs
        if (PlayerPrefs.HasKey(PREFS_KEY))
        {
            string gameType = PlayerPrefs.GetString(PREFS_KEY);
            if (gameType == "OldGame")
            {
                oldGame = true;
                newGame = false;


            }
            else if (gameType == "NewGame")
            {
                newGame = true;
                oldGame = false;

            }
        }
    }

    public void SaveGameType()
    {
        // uložení hodnoty v PlayerPrefs
        if (oldGame)
        {
            PlayerPrefs.SetString(PREFS_KEY, "OldGame");
        }
        else if (newGame)
        {
            PlayerPrefs.SetString(PREFS_KEY, "NewGame");
        }
    }
}