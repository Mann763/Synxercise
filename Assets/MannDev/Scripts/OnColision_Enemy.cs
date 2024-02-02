using UnityEngine;


public class OnColision_Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem EnemyHitParticle;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Enemy_hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.CompareTag("Hands") || other.CompareTag("Legs"))
        {
            LiveManager.Instance.DecreaseLives();
            Vector3 contactPoint = other.ClosestPoint(this.transform.position);
            Vector3 contactNormal = other.transform.up; // You may need to adjust this based on the orientation of your objects

            Instantiate(EnemyHitParticle, contactPoint, Quaternion.LookRotation(contactNormal));

            this.gameObject.AddComponent<AudioSource>().PlayOneShot(Enemy_hit, 0.1f);


            Debug.Log("Life Decreased");

        }
    }
}
