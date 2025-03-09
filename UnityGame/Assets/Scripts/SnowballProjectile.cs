using UnityEngine;

public class SnowballProjectile : MonoBehaviour
{
    [Header("Shattering Effect")]
    public GameObject snowShatterEffect; // Assign in the Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Snowball hit: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        // Check if the collided object has the SnowMan component

        if (collision.gameObject.CompareTag("snowman"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 contactPoint = contact.point;
                CreateShatterEffect(contactPoint);
                Debug.Log("Collision at: " + contactPoint);
            }
            Debug.Log("Hit a Snowman!");
            SnowMan snowman = collision.gameObject.GetComponentInParent<SnowMan>();
            Debug.Log($"snowman: {snowman}");
            Vector3 hitPoint = collision.contacts[0].point;
            if (hitPoint.y > collision.gameObject.transform.position.y + 0.5f) 
            {
                snowman.RegisterHit("head");  // Assume hit above torso is head
            }
            else
            {
                snowman.RegisterHit("torso");
            }
        }
        Destroy(gameObject); // Destroy the snowball on impact
    }

    private void CreateShatterEffect(Vector3 position)
    {
        if (snowShatterEffect)
        {
            GameObject effect = Instantiate(snowShatterEffect, position, Quaternion.identity);
            Destroy(effect, 4f); // Destroy after 2 seconds
        }
    }
}
