using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFlagPointer : MonoBehaviour
{
    [SerializeField] private Transform player;    // The player object
    [SerializeField] private Transform flag;      // The flag/target object
    [SerializeField] private Transform flagIcon;
    [SerializeField] private float distanceFromPlayer = 2.0f; // Distance to hover around player

    private Quaternion originalIconRotation;  // To store the icon's original rotation

    private void Start()
    {
        originalIconRotation = flagIcon.rotation;
    }

    void Update()
    {
        // Get direction from player to flag
        Vector3 direction = (flag.position - player.position).normalized;

        // Set arrow's position relative to player
        transform.position = player.position + direction * distanceFromPlayer;

        // Make the arrow point toward the flag
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Reset the icon's local rotation to keep it upright
        flagIcon.rotation = originalIconRotation;

    }
}
