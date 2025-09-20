using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;
    public int meteorCount = 0;

    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
        InvokeRepeating("SpawnMeteor", 1f, 2f);
    }

    void Update()
    {
        if (gameManagerScript.gameOver)
        {
            CancelInvoke();
        }

        if (meteorCount == 5)
        {
            SpawnBigMeteor();
        }
    }

    void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void SpawnBigMeteor()
    {
        meteorCount = 0;
        Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
        FindObjectOfType<BigMeteorCameraZoom>().OnBigMeteorSpawned();
    }
}
