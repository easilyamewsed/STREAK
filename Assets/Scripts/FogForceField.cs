using UnityEngine;

public class FogPushScript : MonoBehaviour
{
    public float pushStrength = 5f;    // Strength of the push effect
    public float pushRadius = 5f;      // Radius of the effect area
    public ParticleSystem fogParticles; // Reference to the fog particle system

    private ParticleSystem.Particle[] particles;  // Array to hold the particles

    void Start()
    {
        // Initialize the array for particles
        particles = new ParticleSystem.Particle[fogParticles.main.maxParticles];
    }

    void Update()
    {
        // Get all particles from the fog particle system
        int particleCount = fogParticles.GetParticles(particles);

        // Loop through all the particles
        for (int i = 0; i < particleCount; i++)
        {
            // Calculate the direction from the particle to the center (the player)
            Vector3 directionToPlayer = particles[i].position - transform.position;
            float distance = directionToPlayer.magnitude;

            // If the particle is within the radius of the push effect, apply force
            if (distance < pushRadius)
            {
                // Normalize the direction and push the particle away from the player
                Vector3 pushDirection = directionToPlayer.normalized;

                // Apply velocity to the particle to push it away
                particles[i].velocity += pushDirection * pushStrength * Time.deltaTime;
            }
        }

        // Update the particles in the system with the new velocities
        fogParticles.SetParticles(particles, particleCount);
    }
}
