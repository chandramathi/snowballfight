using UnityEngine;

using UnityEngine;

public class SnowballThrower : MonoBehaviour
{
    // public GameObject snowballPrefab; // Assigned the snowball prefab in the inspector
    // public GameObject spawnPoint; 
    public float throwForce = 10f; // Force applied when throwing
    // public int maxSnowballs = 20; // Maximum number of snowballs
    // private int snowballsLeft;
    // private GameObject currentSnowball;

    // void Start()
    // {
    //     Debug.Log("Start ");
    //     snowballsLeft = maxSnowballs;
    //     SpawnNewSnowball();
    //     Debug.Log("Spawned at the Start ");
    // }

    // void Update()
    // {
    //     if(Input.GetMouseButtonDown(0)) // Left mouse button or adapt for VR controller input
    //     {
    //         Debug.Log(" Mouse clicked ");
    //         ThrowSnowball();
    //     }
    // }

    // void SpawnNewSnowball()
    // {
    //     Debug.Log(" SpawnNew snowball ");
    //     if (snowballsLeft > 0)
    //     {
    //         currentSnowball = Instantiate(snowballPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    //         currentSnowball.transform.parent = spawnPoint.transform;
    //         snowballsLeft--;
    //     }
    // }

    // void ThrowSnowball()
    // {
    //     if (currentSnowball == null)
    //     {
    //         Debug.Log("ThrowSnowball called but no snowball to throw.");
    //         currentSnowball = Instantiate(snowballPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    //         currentSnowball.transform.parent = spawnPoint.transform;
    //         snowballsLeft--;
    //     }

    //     Debug.Log($"Throwing snowball: {currentSnowball}");

    //     Rigidbody rb = currentSnowball.GetComponent<Rigidbody>();

    //     if (rb != null)
    //     {
    //         Debug.Log($"Rigidbody found: {rb}");
            
    //         currentSnowball.transform.SetParent(null); // Detach from hand
    //         rb.isKinematic = false; // Enable physics
    //         rb.linearVelocity = spawnPoint.transform.forward * throwForce; // Apply force

    //         Debug.Log($"Applied force: {spawnPoint.transform.forward * throwForce}");
            
    //         Destroy(currentSnowball, 5f); // Destroy after 5 seconds
    //         Invoke(nameof(SpawnNewSnowball), 0.5f); // Spawn new snowball after delay
    //     }
    //     else
    //     {
    //         Debug.LogError("No Rigidbody found on the snowball!");
    //     }
    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Destroy(gameObject);
    // }

    public GameObject snowballPrefab; // Assign the Snowball prefab in Inspector
    public Transform throwPoint; // Assign the wrist or hand transform

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Debug.Log(" Mouse clicked");
            ThrowSnowball();
        }
    }

    void Start()
    {
        if (throwPoint == null)
        {
            throwPoint = GameObject.Find("Wrist")?.transform;
            if (throwPoint == null)
            {
                Debug.LogError("Throw Point (Wrist) not found! Assign it manually in the Inspector.");
            }
        }
    }
    void ThrowSnowball()
    {
        Debug.Log(" ThrowSnowball");
        GameObject snowball = Instantiate(snowballPrefab, throwPoint.position, throwPoint.rotation);
        Debug.Log($" snowball {snowball}");
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            // Debug.Log($" snowball {snowball}");
            rb.AddForce(throwPoint.up * throwForce, ForceMode.Impulse);
            // Destroy(snowball, 5f);
            // Debug.Log($" force {throwForce}");
            // rb.linearVelocity = throwPoint.forward * throwForce; 
            // Debug.Log($" Velocity {rb.linearVelocity}");
            // Debug.DrawRay(throwPoint.position, throwPoint.forward * 5, Color.red, 2f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Snowball hit: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        // Check if the collided object has the SnowMan component

        if (collision.gameObject.CompareTag("snowman"))
        {
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

        // Destroy(gameObject); // Destroy the snowball on impact
    }

}

