using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T��da  Flashlight, kter� m��e b�t zapnuta a vypnuta. Sv�tlo m� �ivotnost v procentech, kter� se postupn� sni�uje, kdy� je sv�tilna zapnuta, a postupn� zvy�uje, kdy� je vypnuta. M��e b�t nastavena rychlost vyb�jen� a nab�jen� baterky.
/// </summary>
public class Flashlight : MonoBehaviour
{
    public Light flashlight; // odkaz na sv�tlo
    public float batteryLife = 100f; // �ivotnost baterky v procentech
    public float batteryDrain = 1f; // m�ra vyb�jen� baterky za sekundu
    public float batteryRecharge = 1f; // m�ra nab�jen� baterky za sekundu
    private bool isOn = false; // stav sv�tla - vypnuto

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            // baterka se vyb�j�, dokud nedos�hne nulov� hodnoty
            if (batteryLife > 0)
            {
                batteryLife -= batteryDrain * Time.deltaTime;
            }
            else
            {
                batteryLife = 0;
                TurnOffFlashlight();
            }
        }
        else
        {
            // baterka se nab�j�, dokud nedos�hne maxim�ln� hodnoty
            if (batteryLife < 100)
            {
                batteryLife += batteryRecharge * Time.deltaTime;
            }
            else
            {
                batteryLife = 100;
            }
        }

        // uprav� intenzitu sv�tla podle �ivotnosti baterky
        float intensity = batteryLife / 100;
        flashlight.intensity = intensity;
    }

    /// <summary>
    /// Metoda pro zapnut� a vypnut� sv�tla.
    /// </summary>
    void ToggleFlashlight()
    {
        isOn = !isOn;
        if (isOn)
        {
            TurnOnFlashlight();
        }
        else
        {
            TurnOffFlashlight();
        }
    }

    /// <summary>
    /// Metoda pro zapnut� sv�tla.
    /// </summary>
    void TurnOnFlashlight()
    {
        flashlight.enabled = true;
    }

    /// <summary>
    /// Metoda pro vypnut� sv�tla.
    /// </summary>
    void TurnOffFlashlight()
    {
        flashlight.enabled = false;
    }
}