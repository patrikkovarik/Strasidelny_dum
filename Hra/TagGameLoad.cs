using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagGameLoad : MonoBehaviour
{
    private const string PREFS_KEY = "gameType"; // konstanta pro ulo�en� kl��e PlayerPrefs
    private bool oldGame = false;
    private bool newGame = false;

    void Start()
    {
        // na�ten� hodnoty ulo�en� v PlayerPrefs p�i spu�t�n� sc�ny
        LoadGameType();
    }

    public void GetTagName()
    {
        if (tag == "OldGame")
        {
            oldGame = true;
            newGame = false;

            SaveGameType(); // ulo�en� hodnoty "OldGame" v PlayerPrefs
        }
        else if (tag == "NewGame")
        {
            newGame = true;
            oldGame = false;

            SaveGameType(); // ulo�en� hodnoty "NewGame" v PlayerPrefs
        }
    }

    public void LoadGameType()
    {
        // na�ten� hodnoty ulo�en� v PlayerPrefs
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
        // ulo�en� hodnoty v PlayerPrefs
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