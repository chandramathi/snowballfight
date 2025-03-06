using UnityEngine;

public class SnowballProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("snowman"))
        {
            SnowMan snowman = collision.gameObject.GetComponentInParent<SnowMan>();
            if (snowman != null)
            {
                // do something
            }
        }

        Destroy(gameObject);
    }
}
