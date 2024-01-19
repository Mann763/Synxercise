using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Transform[] playerTransforms;
    public float moveDuration = 1f;
    private Vector3 originalPosition;

    // Event for notifying move completion
    public event Action OnMoveComplete;

    private void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;
    }

    // This function is now public, so it can be called from the manager script
    public void MoveTowardsPlayerFunction(Action onAllMovesComplete)
    {
        if (playerTransforms == null || playerTransforms.Length == 0)
        {
            Debug.LogError("Player Transforms not assigned!");
            return;
        }

        // Choose a random player position
        Transform randomPlayerTransform = playerTransforms[UnityEngine.Random.Range(0, playerTransforms.Length)];

        // Calculate the direction from the current position to a point slightly beyond the chosen player's position
        Vector3 targetPosition = randomPlayerTransform.position + (randomPlayerTransform.forward * -2f); // Adjust the factor as needed

        // Use DOTween to move the object towards the calculated position
        transform.DOMove(targetPosition, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => NotifyMoveComplete(onAllMovesComplete)); // Callback to notify move completion
    }

    public void ReturnToOriginalPosition(Action onReturnComplete = null)
    {
        transform.DOMove(originalPosition, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnReturnComplete(onReturnComplete));
    }

    private void NotifyMoveComplete(Action onAllMovesComplete)
    {
        // Invoke the move completion event
        OnMoveComplete?.Invoke();

        // Notify the central manager that this object has completed its move
        onAllMovesComplete?.Invoke();
    }

    private void OnReturnComplete(Action onReturnComplete)
    {
        // Invoke the return completion event
        onReturnComplete?.Invoke();
    }
}
