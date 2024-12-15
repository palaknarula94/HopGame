using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformManager _platformManager; // Reference to the PlatformManager

    private const float DeactivationThreshold = 10f; // Distance threshold for deactivation

    private void Awake()
    {
        // Cache the PlatformManager instance for better performance
        _platformManager = FindObjectOfType<PlatformManager>();
        if (_platformManager == null)
        {
            Debug.LogError("PlatformManager not found in the scene!");
        }
    }

    private void Update()
    {
        // Check if the platform is far enough behind the camera to deactivate
        if (IsBehindCamera())
        {
            DeactivatePlatform();
        }
    }

    /// <summary>
    /// Determines if the platform is behind the camera.
    /// </summary>
    /// <returns>True if the platform is behind the camera, false otherwise.</returns>
    private bool IsBehindCamera()
    {
        return transform.position.z < Camera.main.transform.position.z - DeactivationThreshold;
    }

    /// <summary>
    /// Deactivates the platform and returns it to the platform pool.
    /// </summary>
    private void DeactivatePlatform()
    {
        gameObject.SetActive(false);
        _platformManager?.ReturnPlatformToPool(gameObject);
    }
}