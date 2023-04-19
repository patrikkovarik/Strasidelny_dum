using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída  Flashlight, která mùže být zapnuta a vypnuta. Svìtlo má životnost v procentech, která se postupnì snižuje, když je svítilna zapnuta, a postupnì zvyšuje, když je vypnuta. Mùže být nastavena rychlost vybíjení a nabíjení baterky.
/// </summary>
public class Flashlight : MonoBehaviour
{
    public Light flashlight; // odkaz na svìtlo
    public float batteryLife = 100f; // životnost baterky v procentech
    public float batteryDrain = 1f; // míra vybíjení baterky za sekundu
    public float batteryRecharge = 1f; // míra nabíjení baterky za sekundu
    private bool isOn = false; // stav svìtla - vypnuto

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            // baterka se vybíjí, dokud nedosáhne nulové hodnoty
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
            // baterka se nabíjí, dokud nedosáhne maximální hodnoty
            if (batteryLife < 100)
            {
                batteryLife += batteryRecharge * Time.deltaTime;
            }
            else
            {
                batteryLife = 100;
            }
        }

        // upraví intenzitu svìtla podle životnosti baterky
        float intensity = batteryLife / 100;
        flashlight.intensity = intensity;
    }

    /// <summary>
    /// Metoda pro zapnutí a vypnutí svìtla.
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
    /// Metoda pro zapnutí svìtla.
    /// </summary>
    void TurnOnFlashlight()
    {
        flashlight.enabled = true;
    }

    /// <summary>
    /// Metoda pro vypnutí svìtla.
    /// </summary>
    void TurnOffFlashlight()
    {
        flashlight.enabled = false;
    }
}