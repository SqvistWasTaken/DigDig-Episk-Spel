using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponautoatim : MonoBehaviour
{
    /*
    public GameObject projectilePrefab;
    [SerializeField] private bool isPlayer1 = true;
    [SerializeField] private Transform player2;
    [SerializeField] private Transform player1;
    private bool canShoot = true; // Declare canShoot as a class-level variable
    void Update()
    {

        // Hämta knapptryckningar från tangentbordet för att rotera vapnet
        float rotationInput = 0f;

        // Check if the current player is pressing the fire button and can shoot
        if (canShoot && ((isPlayer1 && Input.GetButton("Fire1")) || (!isPlayer1 && Input.GetButton("Fire2"))))
        {
            Shoot();
            canShoot = false; // Set canShoot to false to start cooldown
            Invoke("ResetShootFlag", 0.75f); // Invoke a method to reset the shoot flag after cooldown
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
    */
}
