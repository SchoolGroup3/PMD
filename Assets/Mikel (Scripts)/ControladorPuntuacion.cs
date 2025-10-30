using UnityEngine;

public class ControladorPuntuacion : MonoBehaviour
{
    public int puntuacion = 0;
    public int coleccionablesRecogidos = 0;
    public int coleccionablesTotales = 5; // Ajusta según el nivel

    public void IncrementarPuntuacion(int puntos)
    {
        puntuacion += puntos;
    }

    public void IncrementarColeccionables()
    {
        coleccionablesRecogidos++;
    }

    public bool HaRecogidoTodos()
    {
        return coleccionablesRecogidos >= coleccionablesTotales;
    }
}
