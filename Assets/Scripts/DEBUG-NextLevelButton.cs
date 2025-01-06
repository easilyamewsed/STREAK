using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        // Teleport to different positions based on 1,2,3 keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            player.position = new Vector3(0f, -4.5f, -0.01f);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            player.position = new Vector3(35.25f, -4.5f, -0.01f);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            player.position = new Vector3(71.25f, -4.5f, -0.01f);
    }
}