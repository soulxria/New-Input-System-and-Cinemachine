using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [Header("Star Settings")]
    public GameObject starPrefab;       // Your star prefab
    public int starCount = 100;         // Number of stars to spawn

    [Header("Spawn Area")]
    public Vector2 areaMin = new Vector2(-20f, -10f);
    public Vector2 areaMax = new Vector2(20f, 10f);

    [Header("Optional")]
    public Transform parent;            // Parent transform for spawned stars (optional)

    void Start()
    {
        SpawnStars();
    }

    void SpawnStars()
    {
        if (starPrefab == null)
        {
            Debug.LogWarning("StarSpawner: No star prefab assigned!");
            return;
        }

        for (int i = 0; i < starCount; i++)
        {
            // Generate random position within defined area
            float x = Random.Range(areaMin.x, areaMax.x);
            float y = Random.Range(areaMin.y, areaMax.y);

            Vector3 spawnPos = new Vector3(x, y, 0f); // Z=0 for 2D plane

            // Instantiate the star
            GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);

            // Optional: parent under a container for organization
            if (parent != null)
                star.transform.SetParent(parent);
        }
    }
}
