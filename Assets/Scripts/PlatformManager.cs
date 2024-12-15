using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{
    
    [Header("Platform Prefab Settings")]
    [SerializeField] private GameObject platformPrefab;  // Reference to platform prefab
    [SerializeField] private Transform ballTransform;    // Reference to the ball's transform

    [Header("Platform Spawn Settings")]
    [SerializeField] private int maxPlatforms = 5;       // Total platforms to keep in the scene
    [SerializeField] private float platformSpacingZ = 5.6f; // Distance between platforms Z-axis)
    [SerializeField] private float minX = -4f;            // Minimum X position for random spawning
    [SerializeField] private float maxX = 4f;             // Maximum X position for random spawning

    [Header("Platform Pooling Settings")]
    [SerializeField] private Queue<GameObject> platformPool = new Queue<GameObject>(); // Pool of platforms

    private float lastSpawnZ = 0f;       // Track the last platform's Z position
   

    void Start()
    {
        //Initialize pool with inactive platforms
        for (int i = 0; i < maxPlatforms; i++)
        {
            GameObject platform = Instantiate(platformPrefab);
            platform.SetActive(false); 
            platformPool.Enqueue(platform); 
        }
        SpawnFirstPlatform();
        for (int i = 1; i < maxPlatforms; i++)
        {
            SpawnPlatform();
        }
    }
    void Update()
    {
        if (ballTransform.position.z > lastSpawnZ - (maxPlatforms * platformSpacingZ))
        {
            SpawnPlatform();
        }
    }

    /// <summary>
    /// Spawns the First platforms
    /// </summary>
    void SpawnFirstPlatform()
    {
        GameObject firstPlatform = platformPool.Count > 0 ? platformPool.Dequeue() : Instantiate(platformPrefab);
        
        firstPlatform.transform.position = new Vector3(0, 0, 0);
        firstPlatform.SetActive(true);
        platformPool.Enqueue(firstPlatform);
    }
    /// <summary>
    /// Spawns all platforms and keeps in queue
    /// </summary>
    void SpawnPlatform()
    {
        GameObject platform = platformPool.Count > 0 ? platformPool.Dequeue() : Instantiate(platformPrefab);

        float randomX = Random.Range(minX, maxX);
        lastSpawnZ += platformSpacingZ;
        
        platform.SetActive(true);
        platform.transform.position = new Vector3(randomX, 0, lastSpawnZ);
    }

    // <summary>
    /// Returns a platform to the pool for reuse.
    /// </summary>
    public void ReturnPlatformToPool(GameObject platform)
    {
        if (!platformPool.Contains(platform))
        {
            platformPool.Enqueue(platform);
            platform.SetActive(false); 
        }
    }
}
