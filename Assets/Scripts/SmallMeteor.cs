using UnityEngine;

public class SmallMeteor : Meteor, ICollisionHandler
{
    private CameraShake camerashake;

    protected override void Start()
    {
        // Call the parent's Start method to initialize components
        base.Start();
        
        // Find the CameraShake component in the scene automatically
        GameObject camObj = GameObject.FindGameObjectWithTag("VirCam");
        if (camObj != null)
        {
            camerashake = camObj.GetComponent<CameraShake>();
        }

        if (camerashake == null)
        {
            Debug.LogWarning("SmallMeteor: No CameraShake component found on object tagged 'VirCam'!");
        }
    }

    public void HandleCollision(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            Debug.Log("Destroyed Player");
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
        else if (whatIHit.tag == "Laser")
        {
            // Add camera shake when laser hits small meteor
            if (camerashake != null)
            {
                camerashake.Shake(5f, 0.3f); // Lighter shake than big meteor
                Debug.Log("SmallMeteor: Camera shake triggered!");
            }
            
            GameObject.Find("GameManager").GetComponent<MeteorSpawner>().meteorCount++;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        HandleCollision(whatIHit);
    }
}
