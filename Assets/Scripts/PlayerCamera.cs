using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
    }
}