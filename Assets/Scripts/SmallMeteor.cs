using UnityEngine;

public class SmallMeteor : Meteor, ICollisionHandler
{
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
