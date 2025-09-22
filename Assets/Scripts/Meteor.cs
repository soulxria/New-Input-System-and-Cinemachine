using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Meteor : MonoBehaviour, IMovable
{
    //from Lab 3
    public Transform player;
    public float baseSpeed;
    public float orbitFactor; // sideways vs inward pull
    public float avoidanceStrength = 2f;
    public float avoidanceRadius = 1f;
    public float rotationSpeed = 360f; // degrees per second for turning
    public float moveSmooth = 0.1f; // smoothing factor for direction

    private Rigidbody2D rb;
    private Vector2 smoothDir; // smoothed movement direction
    public float speed;
    public float distance;

    //new variables for dot product rotation
    public float dotProd;
    public float angleMade;
    public float angleMadeDegrees;
    private float angleTracker;

    private Quaternion newRotation;
    bool randomBool;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        smoothDir = transform.up;
        angleTracker = dotProd;
        baseSpeed = Random.Range(2f, 4f);
        orbitFactor = Random.Range(.7f, 1f);
        randomBool = Random.Range(0, 2) == 0;

        FindPlayer();
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        // Safety check for rb component
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is null! Make sure Start() method is called properly.");
            return;
        }

        // Get a local reference to avoid race conditions
        Transform currentPlayer = player;
        if (currentPlayer == null)
        {
            FindPlayer();
            currentPlayer = player;
            if (currentPlayer == null) return; // Exit if player still not found
        }

        Vector2 toPlayer = (currentPlayer.position - transform.position);
        distance = toPlayer.magnitude;
        Vector2 toPlayerNormalized = toPlayer.normalized;

        // Orbit direction (perpendicular to player vector)
        Vector2 orbitDir = new Vector2(-toPlayerNormalized.y, toPlayerNormalized.x);

        // Mix orbit and pull so enemy doesn't drift away
        Vector2 moveDir = (toPlayerNormalized * (1f - orbitFactor)) + (orbitDir * orbitFactor);

        // Speed scales with distance
        speed = baseSpeed + distance * 0.3f;

        // Avoidance from other enemies
        if (randomBool == false)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);
            foreach (Collider2D h in hits)
            {
                if (h != null && h.gameObject != gameObject && h.transform != null)
                {
                    Vector2 away = (transform.position - h.transform.position).normalized;
                    moveDir += away * avoidanceStrength;
                }
            }
        }

        // Normalize final move direction
        moveDir.Normalize();

        // Smooth movement direction (prevents jitter)
        smoothDir = Vector2.Lerp(smoothDir, moveDir, moveSmooth).normalized;

        // Move enemy
        Vector2 newPos = rb.position + smoothDir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // Double-check player still exists before rotation calculations
        if (currentPlayer == null) return;

        //draw dot product btwn vector of player up and the vector from enemy to player
        dotProd = Vector2.Dot((transform.position - currentPlayer.position).normalized, currentPlayer.up);
        angleMade = Mathf.Acos(dotProd); //get the angle
        angleMadeDegrees = angleMade * Mathf.Rad2Deg; //convert to degree

        //dot product is normalized, track whether it is increasing/decreasing
        if (dotProd < angleTracker)
        {
            newRotation = Quaternion.Euler(0, 0, (180 - angleMadeDegrees));
            angleTracker = dotProd;
        }
        if (dotProd > angleTracker)
        {
            newRotation = Quaternion.Euler(0, 0, -180 + angleMadeDegrees);
            angleTracker = dotProd;
        }
        transform.rotation = newRotation; //change rotation
    }
}