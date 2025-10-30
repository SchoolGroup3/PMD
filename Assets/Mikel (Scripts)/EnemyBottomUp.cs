using UnityEngine;

public class EnemyBottomUp : MonoBehaviour
{
    public Vector3 puntoAbajo;
    public Vector3 puntoArriba;
    public float velocidad = 2f;
    private bool subiendo = true;

    void Start()
    {
        transform.position = puntoAbajo;
    }

    void Update()
    {
        if (subiendo)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoArriba, velocidad * Time.deltaTime);
            if (transform.position == puntoArriba)
                subiendo = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoAbajo, velocidad * Time.deltaTime);
            if (transform.position == puntoAbajo)
                subiendo = true;
        }
    }
}
