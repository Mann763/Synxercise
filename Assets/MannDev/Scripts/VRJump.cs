using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class VRJump : MonoBehaviour
{
    public XRInteractionGroup leftHand;
    public XRInteractionGroup rightHand;
    public Camera headController; // Assuming it's a single controller representing the VR headset
    public float jumpThreshold = 0.2f;
    public float speedThreshold = 1.0f; // Adjust as needed
    public float jumpHeight = 0.3f; // Adjust the virtual jump height
    public float smoothTime = 0.1f; // Smooth time for height adjustment
    public float gravity = 9.8f; // Adjust gravity strength
    public float groundCheckDistance = 0.2f; // Adjust distance for ground check
    public LayerMask groundLayer; // Specify the ground layer

    private float initialHeight;
    private bool isJumping = false;
    private float jumpStartHeight;
    private float lastTime;
    private CharacterController characterController;

    void Start()
    {
        // Store the initial height of the player
        initialHeight = GetAverageControllerHeight();

        // Initialize CharacterController (assuming it's attached to the same GameObject)
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Apply gravity
        ApplyGravity();

        // Check for the jump gesture (VR)
        if (CheckForJumpGesture())
        {
            // Start the jump
            Jump();
        }

        // Check for testing jump without VR (using spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Start the jump
            Jump();
        }

        // If jumping, smoothly adjust the height to simulate the jump
        if (isJumping)
        {
            float timeSinceJumpStart = Time.time - lastTime;
            float t = Mathf.SmoothStep(0, 1, timeSinceJumpStart / smoothTime);
            float newHeight = Mathf.Lerp(jumpStartHeight, initialHeight, t);

            // Set the new height of the VR rig or headset
            // Adjust based on your VR setup
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);

            // End the jump when the height adjustment is complete
            if (t >= 1.0f)
            {
                isJumping = false;
            }
        }
    }

    void ApplyGravity()
    {
        // Apply gravity to the character controller
        characterController.Move(Vector3.down * gravity * Time.deltaTime);

        // Check if the player is close to the ground
        if (IsGrounded())
        {
            // Reset the vertical velocity if on the ground
            characterController.Move(Vector3.down * characterController.velocity.y * Time.deltaTime);
        }
    }

    bool IsGrounded()
    {
        // Perform a sphere cast to check if the player is close to the ground
        RaycastHit hit;
        return Physics.SphereCast(
            transform.position + characterController.center,
            characterController.radius,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayer);
    }

    bool CheckForJumpGesture()
    {
        // Calculate the current average controller height and head position
        float currentHeight = GetAverageControllerHeight();
        float headPosition = headController.transform.position.y;

        // Calculate the change in height
        float heightChange = currentHeight - initialHeight;

        // Calculate the speed of height change
        float speed = Mathf.Abs(heightChange / (Time.time - lastTime));

        // Check if the height change exceeds the threshold, the speed is high, and the head is not too low
        return heightChange > jumpThreshold && speed > speedThreshold && IsGrounded() && headPosition > transform.position.y;
    }

    void Jump()
    {
        // Record the starting height of the jump
        jumpStartHeight = transform.position.y;

        // Set the jumping flag
        isJumping = true;

        // Record the time of the jump start
        lastTime = Time.time;

        // Apply an upward force to the character controller to initiate the jump
        characterController.Move(Vector3.up * jumpHeight);
    }

    float GetAverageControllerHeight()
    {
        // Check if VR controllers are available
        if (leftHand != null && rightHand != null)
        {
            // Calculate the average height of the left and right controllers
            float averageHeight = (leftHand.transform.position.y + rightHand.transform.position.y) / 2f;
            return averageHeight;
        }
        else
        {
            // Return a default value (for testing without VR)
            return 0.0f;
        }
    }
}
