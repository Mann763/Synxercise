using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class OnColision_Point : MonoBehaviour
{
    [SerializeField] private int ScoreToAdd = 1;
    [SerializeField] private ParticleSystem PointParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hands"))
        {
            Debug.Log("Trigger entered by Hands");
            // Check if ScoreManager is instantiated
            if (ScoreManager.Instance != null)
            {
                // Add points and log
                ScoreManager.Instance.AddPoints(ScoreToAdd);
                Instantiate(PointParticle, other.ClosestPoint(this.transform.position),Quaternion.identity);
                Debug.Log("Points added: " + ScoreToAdd);

            }
            else
            {
                Debug.LogError("ScoreManager not found!");
            }
            // Destroy the GameObject with this script
            Destroy(gameObject);
        }
    }
}
