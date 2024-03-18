using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponautoatim : MonoBehaviour
{
    public GameObject projectilePrefab;
    [SerializeField] private bool isPlayer1 = true;
    [SerializeField] private Transform player2;
    [SerializeField] private Transform player1;
    private bool canShoot = true; // Declare canShoot as a class-level variable

    public Transform playerTransform;
    [SerializeField] public float maxDistance = 10f;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer; // Layer mask for obstacles that should block the raycast
    [SerializeField] public float aimAssistStrength = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current player is pressing the fire button and can shoot
        if (canShoot && ((isPlayer1 && Input.GetButton("Fire1")) || (!isPlayer1 && Input.GetButton("Fire2"))))
        {
            Shoot();
            canShoot = false; // Set canShoot to false to start cooldown
            Invoke("ResetShootFlag", 0.75f); // Invoke a method to reset the shoot flag after cooldown
        }

        AimAssistTowardsEnemy();
    }

    void AimAssistTowardsEnemy()
    {
        // Cast a ray in the direction the player is aiming
        RaycastHit2D[] hits = Physics2D.RaycastAll(playerTransform.position, playerTransform.right, maxDistance, enemyLayer);

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            float distanceToEnemy = Vector2.Distance(playerTransform.position, hitObject.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                // Check if there are any obstacles between player and enemy
                bool isObstacleBetween = Physics2D.Raycast(playerTransform.position, (hitObject.transform.position - playerTransform.position).normalized, distanceToEnemy, obstacleLayer);

                if (!isObstacleBetween)
                {
                    closestEnemy = hitObject;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            // Adjust player's aiming direction towards the closest enemy
            Vector3 directionToEnemy = (closestEnemy.transform.position - playerTransform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(playerTransform.right, directionToEnemy, aimAssistStrength);
            playerTransform.right = newDirection;
        }
    }
    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Set the projectile's direction based on the player's facing direction
        Vector2 playerDirection = transform.right;
        projectile.GetComponent<Rigidbody2D>().velocity = playerDirection * projectile.GetComponent<BulletScript>().speed;

        // Destroy the bullet after 2 seconds
        Destroy(projectile, 2f);
    }

    void ResetShootFlag()
    {
        canShoot = true; // Reset the canShoot flag after cooldown
    }
}
