using UnityEngine;

public class OnColision_Point : MonoBehaviour
{
    [SerializeField] private int ScoreToAdd = 1;
    [SerializeField] private ParticleSystem PointParticle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Point_hit;

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
                Vector3 contactPoint = other.ClosestPoint(this.transform.position);

                Instantiate(PointParticle, contactPoint,Quaternion.identity);
                
                this.gameObject.AddComponent<AudioSource>().PlayOneShot(Point_hit, 0.01f);

                Debug.Log("Points added: " + ScoreToAdd);

            }
            else
            {
                Debug.LogError("ScoreManager not found!");
            }
            // Destroy the GameObject with this script
            Destroy(gameObject,0.5f);
        }
    }
}
