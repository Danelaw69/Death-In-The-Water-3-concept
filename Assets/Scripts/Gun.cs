using UnityEngine;

public class Gun : MonoBehaviour
{
    public float smooth = 10f; // Adjust this value for desired sway speed
    public float swayMultiplier;



    public float raycastDistance = 10f;
    public ParticleSystem particleEffect; // Assign your particle system here

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY * Quaternion.Euler(0f, 0f, -90f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);




        // Check if the left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray that starts at the object's position and points forward
            Ray ray = new Ray(transform.position, transform.forward);

            // Perform the raycast
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
            {
                // Calculate the rotation based on the hit normal
                Quaternion rotation = Quaternion.LookRotation(hit.normal);

                // Instantiate the particle effect at the hit point with the calculated rotation
                ParticleSystem instance = Instantiate(particleEffect, hit.point, rotation);

                // Destroy the particle effect after it's done playing
                Destroy(instance.gameObject, instance.main.duration);
            }
        }
    }
}