using UnityEngine;
using System.Collections;

public class PenguinReaction : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // If a snowball hits this penguin
        if (collision.gameObject.CompareTag("Snowball"))
        {
            Debug.Log("Penguin was hit by a snowball! Spinning...");
            StartCoroutine(SpinPenguin());
        }
    }

    // Spins the penguin 5 full rotations (each 360 degrees) around the Y-axis.
    IEnumerator SpinPenguin()
    {
        int totalRotations = 5;       
        float degreesPerSecond = 360; 

        for (int i = 0; i < totalRotations; i++)
        {
            float rotatedSoFar = 0f;
            while (rotatedSoFar < 360f)
            {
                float spinThisFrame = degreesPerSecond * Time.deltaTime;
                
                // rotate around Y-axis
                transform.Rotate(0f, spinThisFrame, 0f);

                rotatedSoFar += spinThisFrame;
                yield return null;
            }
        }

        Debug.Log("Done spinning!");
    }
}
