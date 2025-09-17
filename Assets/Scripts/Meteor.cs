using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        smoothDir = transform.up; // start pointing "up"
        angleTracker = dotProd;
        baseSpeed = Random.Range(2f, 4f);
        orbitFactor = Random.Range(.7f, 1f);
        randomBool = Random.Range(0, 2) == 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 toPlayer = (player.position - transform.position);
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
                if (h != null && h.gameObject != gameObject)
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

        // Rotate smoothly toward smoothed movement direction
        /*
        float targetAngle = Mathf.Atan2(smoothDir.y, smoothDir.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(
        transform.eulerAngles.z,
        targetAngle,
        rotationSpeed * Time.fixedDeltaTime
        );
        transform.rotation = Quaternion.Euler(0, 0, angle);
        */

        //draw dot product btwn vector of player up and the vector from enemy to player
        dotProd = Vector2.Dot((this.transform.position - player.position).normalized, player.up);
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

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        } else if (whatIHit.tag == "Laser")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().meteorCount++;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }
}
