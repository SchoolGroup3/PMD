using UnityEngine;

public class RotatingBar : MonoBehaviour
{

    public GameObject circles;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(this.transform.parent.gameObject.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
