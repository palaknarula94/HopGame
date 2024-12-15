using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform player; // Reference to the player's transform

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Default offset position from the player
    [SerializeField] private float smoothSpeed = 5f;                 // Camera follow smoothness

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned to CameraFollow.");
            return;
        }

        // Calculate the desired position based on player's position and offset
        Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, player.position.z + offset.z);

        // Smoothly interpolate between the current and desired positions
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}