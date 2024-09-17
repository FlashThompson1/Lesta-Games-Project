using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInt : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer;
    private Rigidbody _rigidbody;
    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;
    private bool isOnPlatform = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandlePlatformMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // Tag for the platform
        {
            currentPlatform = collision.transform;
            lastPlatformPosition = currentPlatform.position;
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == currentPlatform)
        {
            isOnPlatform = false;
            currentPlatform = null;
        }
    }

    private void HandlePlatformMovement()
    {
        if (isOnPlatform && currentPlatform != null)
        {
            // Calculate platform's movement
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;

            // Move the player with the platform
            _rigidbody.MovePosition(_rigidbody.position + platformMovement);

            // Update the last known position of the platform
            lastPlatformPosition = currentPlatform.position;

            // Check if the player is near the edge of the platform
            if (IsPlayerNearEdge())
            {
                // If near the edge, detach from the platform
                isOnPlatform = false;
                currentPlatform = null;
            }
        }
    }

    private bool IsPlayerNearEdge()
    {
        // Get the bounds of the platform
        Collider platformCollider = currentPlatform.GetComponent<Collider>();
        Vector3 platformBoundsMin = platformCollider.bounds.min;
        Vector3 platformBoundsMax = platformCollider.bounds.max;

        // Check if the player is outside the platform's bounds
        Vector3 playerPosition = transform.position;

        // If the player is outside the bounds, they should fall off
        if (playerPosition.x < platformBoundsMin.x || playerPosition.x > platformBoundsMax.x ||
            playerPosition.z < platformBoundsMin.z || playerPosition.z > platformBoundsMax.z)
        {
            return true;
        }

        return false;
    }
}