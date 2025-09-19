using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;
    public bool gameOver = false;

    public int meteorCount = 0;

    private PlayerMovement controls; 

    void Awake()
    {
        controls = new PlayerMovement(); 
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Gameplay.Restart.performed += OnRestart; // listen for restart input
    }

    void OnDisable()
    {
        controls.Gameplay.Restart.performed -= OnRestart;
        controls.Disable();
    }

    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnMeteor", 1f, 2f);
    }

    void Update()
    {
        if (gameOver)
        {
            CancelInvoke();
        }

        if (meteorCount == 5)
        {
            BigMeteor();
        }
    }

    
    private void OnRestart(InputAction.CallbackContext context)
    {
        if (gameOver)
        {
            SceneManager.LoadScene("Week5Lab");
        }
    }

    void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void BigMeteor()
    {
        meteorCount = 0;
        Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }
}
