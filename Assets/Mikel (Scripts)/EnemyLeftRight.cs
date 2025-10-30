using UnityEngine;

public class EnemyLeftRight : MonoBehaviour
{
    public Vector3 puntoIzquierda;
    public Vector3 puntoDerecha;
    public float velocidad = 2f;
    private bool moviendoDerecha = true;

    void Start()
    {
        transform.position = puntoIzquierda;
    }

    void Update()
    {
        if (moviendoDerecha)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDerecha, velocidad * Time.deltaTime);
            if (transform.position == puntoDerecha)
                moviendoDerecha = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoIzquierda, velocidad * Time.deltaTime);
            if (transform.position == puntoIzquierda)
                moviendoDerecha = true;
        }
    }
}
