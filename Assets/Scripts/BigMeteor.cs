using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMeteor : Meteor, ICollisionHandler
{
    private int hitCount = 0;
    private CameraShake camerashake;

    protected override void Start()
    {
        // Call the parent's Start method to initialize rb and other components
        base.Start();
        
        // Find the CameraShake component in the scene automatically
        GameObject camObj = GameObject.FindGameObjectWithTag("VirCam");
        if (camObj != null)
        {
            camerashake = camObj.GetComponent<CameraShake>();
        }

        if (camerashake == null)
        {
            Debug.LogError("No CameraShake component found on object tagged 'VirCam'!");
        }
    }

    // Handles collision of big meteor
    public void HandleCollision(Collider2D whatIHit)
    {
        if (whatIHit.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            Destroy(whatIHit.gameObject);
        }
        else if (whatIHit.CompareTag("Laser"))
        {
            Debug.Log("BigMeteor: Laser collision detected!");
            
            if (camerashake != null)
            {
                camerashake.Shake(5f, 2f);
                Debug.Log("BigMeteor: Camera shake triggered!");
            }
            else
            {
                Debug.LogError("BigMeteor: CameraShake component is null!");
            }

            hitCount++;
            Destroy(whatIHit.gameObject);

            if (hitCount >= 5) // Big meteor requires 5 hits to destroy
            {
                Debug.Log("BigMeteor: Destroyed after 5 hits!");
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        HandleCollision(whatIHit);
    }
}
