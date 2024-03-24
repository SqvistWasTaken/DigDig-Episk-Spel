using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    [SerializeField] private float rotationSpeed = 50f; // Justera för rotationshastighet
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

        // Kontrollera om knapp för högerrotation är nedtryckt
        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.E))
            {
                rotationInput = 1f;
            }
            // Kontrollera om knapp för vänsterrotation är nedtryckt
            else if (Input.GetKey(KeyCode.Q))
            {
                rotationInput = -1f;
            }
        }

        if (!isPlayer1)
        {
            if (Input.GetKey(KeyCode.O))
            {
                rotationInput = 1f;
            }
            // Kontrollera om knapp för vänsterrotation är nedtryckt
            else if (Input.GetKey(KeyCode.I))
            {
                rotationInput = -1f;
            }
        }

        // Beräkna rotationshastighet baserat på knapptryckningar
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        //Lägger rotation på vapnet
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Set the projectile's direction based on the player's facing direction
        Vector2 playerDirection = transform.right;
        //projectile.GetComponent<Rigidbody2D>().velocity = playerDirection * projectile.GetComponent<BulletScript>().speed;

        // Destroy the bullet after 2 seconds
        Destroy(projectile, 2f);
    }


    void ResetShootFlag()
    {
        canShoot = true; // Reset the canShoot flag after cooldown
    }
}
