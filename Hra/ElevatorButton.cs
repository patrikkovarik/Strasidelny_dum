using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script pro ovl�d�n� tla��tek v�tahu.
/// </summary>
public class ElevatorButton : MonoBehaviour
{
    public ElevatorController elevatorController; // Odkaz na script ��d�c� v�tah
    public int endpointIndex; // Index endpointu, na kter� se m� nastavit c�lov� pozice

    /// <summary>
    /// Metoda volan� po kliknut� na tla��tko v�tahu. Nastav� c�lovou pozici v�tahu na z�klad� indexu endpointu.
    /// </summary>
    private void OnMouseDown()
    {
        elevatorController.SetTarget(endpointIndex);
    }
}