using UnityEngine;

public class RotatingBar : MonoBehaviour
{

    public GameObject circles;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(this.transform.parent.gameObject.transform.position, new Vector3(0, 0, 1), 90 * Time.deltaTime);
    }
}
