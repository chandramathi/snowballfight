using UnityEngine;
using System.Collections.Generic;

public class SnowballProjectile : MonoBehaviour
{
    [Header("Shattering Effect")]
    public GameObject snowShatterEffect; // Assign in the Inspector

    [Header("Collision Sounds")]
    public AudioClip snowHitSound1; 
    public AudioClip snowHitSound2; 

    public AudioClip snowHitSound3; 

    public AudioClip penguinShreak1; 
    public AudioClip penguinShreak2; 
    public AudioClip penguinShreak3; 

    private List<AudioClip> snowHitSound;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Add an AudioSource dynamically
        snowHitSound = new List<AudioClip>();
        // audioSource.clip = snowHitSound1;
        // audioSource.playOnAwake = true;  // Auto-plays sound at start
        // audioSource.Play();
        snowHitSound.Add(snowHitSound1);
        snowHitSound.Add(snowHitSound2);
        snowHitSound.Add(snowHitSound3);
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
            GameManager gameManager = FindObjectOfType<GameManager>(); // Find the GameManager instance in the scene
            if (gameManager != null)
            {
                gameManager.IncrementScore();
            }
            if (snowHitSound.Count > 0)
            {
                int index= Random.Range(0, snowHitSound.Count);
                AudioClip randomSound = snowHitSound[index];
                Debug.Log($"Sound effect {index}");
                audioSource.volume = 1.0f;
                audioSource.clip = randomSound;
                audioSource.Play();
                audioSource.clip = null;
            }
        }else if(collision.gameObject.CompareTag("penguin")){
            Debug.Log("Hit a penguin!");
            GameManager gameManager = FindObjectOfType<GameManager>(); // Find the GameManager instance in the scene
            if (gameManager != null)
            {
                gameManager.ReduceLife(); // Call the method on the GameManager instance
            }
            else
            {
                Debug.LogError("GameManager not found in the scene.");
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
