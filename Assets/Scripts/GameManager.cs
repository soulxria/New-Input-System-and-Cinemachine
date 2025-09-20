using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public bool gameOver = false;

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
    }
    
    private void OnRestart(InputAction.CallbackContext context)
    {
        if (gameOver)
        {
            SceneManager.LoadScene("Week5Lab");
        }
    }
}
