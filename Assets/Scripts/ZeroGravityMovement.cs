using UnityEngine;

public class ZeroGravityMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float friction = 0.1f;
    public float momentumFactor = 0.005f;

    private Vector3 velocity = Vector3.zero;

    public Transform cameraTransform;
    public float cameraSensitivity = 2f;

    private Quaternion cameraRotation = Quaternion.identity;
    private Vector2 cameraVelocity = Vector2.zero; // Camera velocity
    public float cameraMomentum = 0.01f; // Momentum factor (0 to 1)
    public float cameraFriction = 0.1f; // Friction factor (0 to 1)
    public float cameraOvershoot = 1.4f; // Overshoot factor (greater than 1)

    void Update()
    {
        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float upDownInput = Input.GetAxis("Up") - Input.GetAxis("Down");

        // Get the camera's forward and right directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        Vector3 up = cameraTransform.up;

        // Calculate movement vector based on camera's rotation
        Vector3 movementVector = (forward * verticalInput + right * horizontalInput + up * upDownInput);

        // Apply momentum to velocity in all directions
        velocity.x = Mathf.Lerp(velocity.x, movementVector.x * movementSpeed, momentumFactor);
        velocity.y = Mathf.Lerp(velocity.y, movementVector.y * movementSpeed, momentumFactor);
        velocity.z = Mathf.Lerp(velocity.z, movementVector.z * movementSpeed, momentumFactor);

        // Apply friction to slow down velocity in all directions
        velocity *= (1f - friction * Time.deltaTime);

        // Move the player
        transform.position += velocity * Time.deltaTime;

        // Camera Movement
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        // Apply momentum to camera velocity
        cameraVelocity.x = Mathf.Lerp(cameraVelocity.x, mouseX * cameraOvershoot, cameraMomentum);
        cameraVelocity.y = Mathf.Lerp(cameraVelocity.y, mouseY * cameraOvershoot, cameraMomentum);

        // Apply friction to slow down camera velocity
        cameraVelocity *= (1f - cameraFriction * Time.deltaTime);

        // Rotate the camera based on its own orientation and velocity
        cameraRotation *= Quaternion.Euler(-cameraVelocity.y, cameraVelocity.x, 0f);

        // Apply the rotation to the camera
        cameraTransform.localRotation = cameraRotation;
    }
}