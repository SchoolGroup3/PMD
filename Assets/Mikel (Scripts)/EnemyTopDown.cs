using UnityEngine;

public class EnemyTopDown : MonoBehaviour
{
    public Vector3 puntoArriba;
    public Vector3 puntoAbajo;
    public float velocidad = 2f;
    private bool bajando = true;

    void Start()
    {
        transform.position = puntoArriba;
    }

    void Update()
    {
        if (bajando)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoAbajo, velocidad * Time.deltaTime);
            if (transform.position == puntoAbajo)
                bajando = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoArriba, velocidad * Time.deltaTime);
            if (transform.position == puntoArriba)
                bajando = true;
        }
    }
}
