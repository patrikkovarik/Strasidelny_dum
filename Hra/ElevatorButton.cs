using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script pro ovládání tlaèítek výtahu.
/// </summary>
public class ElevatorButton : MonoBehaviour
{
    public ElevatorController elevatorController; // Odkaz na script øídící výtah
    public int endpointIndex; // Index endpointu, na který se má nastavit cílová pozice

    /// <summary>
    /// Metoda volaná po kliknutí na tlaèítko výtahu. Nastaví cílovou pozici výtahu na základì indexu endpointu.
    /// </summary>
    private void OnMouseDown()
    {
        elevatorController.SetTarget(endpointIndex);
    }
}