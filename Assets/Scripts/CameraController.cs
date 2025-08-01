using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player GameObject.
    public GameObject player;

    // The distance between the camera and the player.
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Try to find the player if not assigned manually
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer;
            }
        }

        // Always calculate the offset if player exists
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogWarning("CameraController could not find the player!");
        }
    }

    // LateUpdate is called once per frame AFTER all Update functions have been completed.
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
