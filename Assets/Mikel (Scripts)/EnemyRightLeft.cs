using UnityEngine;

public class EnemyRightLeft : MonoBehaviour
{
    public Vector3 puntoDerecha;
    public Vector3 puntoIzquierda;
    public float velocidad = 2f;
    private bool moviendoIzquierda = true;

    void Start()
    {
        transform.position = puntoDerecha;
    }

    void Update()
    {
        if (moviendoIzquierda)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoIzquierda, velocidad * Time.deltaTime);
            if (transform.position == puntoIzquierda)
                moviendoIzquierda = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDerecha, velocidad * Time.deltaTime);
            if (transform.position == puntoDerecha)
                moviendoIzquierda = true;
        }
    }
}
