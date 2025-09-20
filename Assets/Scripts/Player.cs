using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour, IMovable
{
    public GameObject laserPrefab;

    private float speed = 6f;
    private float horizontalScreenLimit = 10f;
    private float verticalScreenLimit = 6f;
    private bool canShoot = true;

    private PlayerMovement controls;    
    private Vector2 moveInput;

    [SerializeField] private float worldXLimit = 20f;
    [SerializeField] private float worldYLimit = 12f;

    void Awake()
    {
        controls = new PlayerMovement();
    }

    void OnEnable()
    {
        controls.Enable();

        // movement input
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        // shooting input
        controls.Gameplay.Shoot.performed += OnShoot;
    }

    void OnDisable()
    {
        controls.Gameplay.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled -= ctx => moveInput = Vector2.zero;
        controls.Gameplay.Shoot.performed -= OnShoot;

        controls.Disable();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime * speed);

        if (transform.position.x > worldXLimit)
            transform.position = new Vector3(-worldXLimit, transform.position.y, 0);

        if (transform.position.x < -worldXLimit)
            transform.position = new Vector3(worldXLimit, transform.position.y, 0);

        if (transform.position.y > worldYLimit)
            transform.position = new Vector3(transform.position.x, -worldYLimit, 0);

        if (transform.position.y < -worldYLimit)
            transform.position = new Vector3(transform.position.x, worldYLimit, 0);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (canShoot)
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            canShoot = false;
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
