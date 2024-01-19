using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class OnColision_Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem EnemyHitParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.CompareTag("Hands") || other.CompareTag("Legs"))
        {
            LiveManager.Instance.DecreaseLives();
            Vector3 contactPoint = other.ClosestPoint(this.transform.position);
            Vector3 contactNormal = other.transform.up; // You may need to adjust this based on the orientation of your objects

            Instantiate(EnemyHitParticle, contactPoint, Quaternion.LookRotation(contactNormal));

            Debug.Log("Life Decreased");

        }
    }
}
