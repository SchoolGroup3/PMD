using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    public int puntos = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ControladorPuntuacion controlador = FindObjectOfType<ControladorPuntuacion>();
            if (controlador != null)
            {
                controlador.IncrementarPuntuacion(puntos);
                controlador.IncrementarColeccionables();
            }

            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            Destroy(gameObject, 0.1f);
        }
    }
}
