using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementManager : MonoBehaviour
{
    public Transform[] playerTransforms;
    public float delayBetweenMovements = 0.5f;
    private MoveTowardsPlayer[] moveScripts;
    private int completedMovementsCount = 0;

    private void Start()
    {
        if (playerTransforms == null || playerTransforms.Length == 0)
        {
            Debug.LogError("Player Transforms not assigned!");
            return;
        }

        // Get all objects with MoveTowardsPlayer script in the scene
        moveScripts = FindObjectsOfType<MoveTowardsPlayer>();

        // Example: Enable the movement of objects towards a random player position with a starting delay
        StartCoroutine(MoveObjectsOneByOneRandomlyWithDelay());
    }

    private IEnumerator MoveObjectsOneByOneRandomlyWithDelay()
    {
        yield return new WaitForSeconds(5f);

        // Shuffle the array randomly
        System.Random rand = new System.Random();
        int n = moveScripts.Length;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            MoveTowardsPlayer temp = moveScripts[k];
            moveScripts[k] = moveScripts[n];
            moveScripts[n] = temp;
        }

        // Reset the completedMovementsCount
        completedMovementsCount = 0;

        // Enable movement one by one with a delay towards a random player position
        foreach (MoveTowardsPlayer moveScript in moveScripts)
        {
            // Check if the script is enabled before moving the object
            if (moveScript.enabled)
            {
                // Set the available player positions in the MoveTowardsPlayer script
                moveScript.playerTransforms = playerTransforms;
                moveScript.OnMoveComplete += OnMoveCompleteCallback; // Subscribe to move completion event
                moveScript.MoveTowardsPlayerFunction(() => OnAllMovesCompleteCallback()); // Pass a callback for individual move completion
                yield return new WaitForSeconds(delayBetweenMovements);
            }
        }
    }

    private void OnMoveCompleteCallback()
    {
        // Increment the completedMovementsCount
        completedMovementsCount++;
    }

    private void OnAllMovesCompleteCallback()
    {
        // Check if all objects have completed their movements
        if (completedMovementsCount == moveScripts.Length)
        {
            // All objects are behind the player, return them to their original positions
            StartCoroutine(ReturnObjectsToOriginalPosition());
        }
    }

    private IEnumerator ReturnObjectsToOriginalPosition()
    {
        foreach (MoveTowardsPlayer moveScript in moveScripts)
        {
            // Check if the script is enabled before moving the object
            if (moveScript.enabled)
            {
                yield return StartCoroutine(MoveObjectToOriginalPosition(moveScript));
                yield return new WaitForSeconds(delayBetweenMovements);
            }
        }

        // All objects have returned to their original positions, restart the movement sequence
        StartCoroutine(MoveObjectsOneByOneRandomlyWithDelay());
    }

    private IEnumerator MoveObjectToOriginalPosition(MoveTowardsPlayer moveScript)
    {
        // Move the object back to its original position
        moveScript.ReturnToOriginalPosition();

        // Wait for the movement to complete
        while (!moveScript.enabled)
        {
            yield return null;
        }
    }

}
