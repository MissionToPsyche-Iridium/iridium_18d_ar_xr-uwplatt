using UnityEngine;

/// <summary>
/// Controls the orbital movement of a satellite object around an asteroid
/// </summary>
public class orbit : MonoBehaviour
{
    public Transform satellite;  // Assign the satellite GameObject
    public Transform asteroid;   // Assign the asteroid GameObject
    public float orbitSpeed = 10f;  // Speed of satellite orbit
    public float orbitRadius = 5f;  // Distance from the asteroid
    public float asteroidRotationSpeed = 2f; // Speed of asteroid rotation
    private float angle = 0f; // Tracks orbit position

    /// <summary>
    /// Initializes the satellite position relative to the asteroid
    /// </summary>
    void Start()
    {
        if (satellite == null || asteroid == null)
        {
            Debug.LogError("Satellite or Asteroid not assigned!");
            return;
        }
        // Correctly position the satellite outside the asteroid at the orbit radius
        satellite.localPosition = asteroid.localPosition + new Vector3(orbitRadius, 0, 0);
    }

    /// <summary>
    /// Updates the satellite orbit and asteroid rotation each frame
    /// </summary>
    void Update()
    {
        if (satellite == null || asteroid == null) return;
        asteroid.rotation *= Quaternion.Euler(0, 0, asteroidRotationSpeed * Time.deltaTime);    // Rotate the asteroid slowly while keeping its X position fixed
        angle += orbitSpeed * Time.deltaTime;   // Update the angle over time for smooth orbit movement

        // Compute new satellite position using circular orbit formula
        float x = asteroid.localPosition.x + Mathf.Cos(angle) * orbitRadius;
        float z = asteroid.localPosition.z + Mathf.Sin(angle) * orbitRadius;
        float y = satellite.localPosition.y; // Keep Y unchanged for a level orbit

        // Apply new position to the satellite
        satellite.localPosition = new Vector3(x, y, z);

        // Ensure the satellite always faces the asteroid
        satellite.LookAt(asteroid);
    }
}