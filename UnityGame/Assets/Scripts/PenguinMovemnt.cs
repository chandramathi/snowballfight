using UnityEngine;
using System.Collections;

public class PenguinMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float changeDirectionInterval = 3f;   

    [Header("Iceberg Reference")]
    public GameObject icebergObject; 

    private BoxCollider icebergCollider;
    private float minX, maxX, minZ, maxZ;
    private Vector3 moveDirection; 

    public float avoidDistance = 1.0f;
    public float avoidForce = 1.0f;

    void Start()
    {
        SetRandomDirection();
        StartCoroutine(ChangeDirection());

        if (icebergObject != null)
        {
            icebergCollider = icebergObject.GetComponent<BoxCollider>();
            if (icebergCollider != null)
            {
                Bounds bounds = icebergCollider.bounds;
                minX = bounds.min.x;
                maxX = bounds.max.x;
                minZ = bounds.min.z;
                maxZ = bounds.max.z;
            }
        }
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);

            // bias movement toward the center if near the boundary
            if (IsNearBoundary(transform.position))
            {
                SetDirectionTowardCenter();
            }
            else
            {
                SetRandomDirection();
            }
        }
    }

    bool IsNearBoundary(Vector3 position)
    {
        float threshold = 0.5f;
        return (position.x < minX + threshold || position.x > maxX - threshold ||
                position.z < minZ + threshold || position.z > maxZ - threshold);
    }

    void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
    }

    void SetDirectionTowardCenter()
    {
        Vector3 center = new Vector3((minX + maxX) / 2, transform.position.y, (minZ + maxZ) / 2);
        Vector3 toCenter = (center - transform.position).normalized;
        
        // add a bit of randomness
        float randomAngle = Random.Range(-30f, 30f);
        moveDirection = Quaternion.Euler(0, randomAngle, 0) * toCenter;
    }

    void Update()
    {
        if (icebergCollider == null) return;

        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

        if (newPosition.x < minX || newPosition.x > maxX ||
            newPosition.z < minZ || newPosition.z > maxZ)
        {
            SetDirectionTowardCenter();
            newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        }

        transform.position = newPosition;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

    void AvoidOtherPenguins()
    {
        // Find all penguins in the scene by tag
        GameObject[] penguins = GameObject.FindGameObjectsWithTag("Penguin");

        // We'll accumulate a "push away" vector
        Vector3 avoidanceVector = Vector3.zero;
        int closePenguinCount = 0;

        foreach (GameObject p in penguins)
        {
            // Skip checking against ourself
            if (p == this.gameObject) continue;

            float distance = Vector3.Distance(p.transform.position, transform.position);
            if (distance < avoidDistance)
            {
                closePenguinCount++;
                // Push away from the other penguin (the closer, the stronger the push)
                Vector3 pushDir = (transform.position - p.transform.position).normalized;
                // Optionally scale by how close they are, e.g. 1/distance
                avoidanceVector += pushDir * (avoidForce / distance);
            }
        }

        // If at least one penguin is close, adjust our moveDirection
        if (closePenguinCount > 0)
        {
            avoidanceVector.y = 0f; // stay on the XZ plane
            // Add this avoidance vector to our current direction
            moveDirection += avoidanceVector;
            // Normalize so we don't get super-fast speeds
            moveDirection.Normalize();
        }
    }
}
