using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    [SerializeField] private float rotationSpeed = 50f; // Justera f�r rotationshastighet
    [SerializeField] private bool isPlayer1 = true;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform player2;
    [SerializeField] private Transform player1;

    void Update()
    {
        // H�mta knapptryckningar fr�n tangentbordet f�r att rotera vapnet
        float rotationInput = 0f;

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }

        else if (Input.GetButton("Fire2"))
        {
            Shoot();
        }

        // Kontrollera om knapp f�r h�gerrotation �r nedtryckt
        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.E))
            {
                rotationInput = 1f;
            }
            // Kontrollera om knapp f�r v�nsterrotation �r nedtryckt
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
            // Kontrollera om knapp f�r v�nsterrotation �r nedtryckt
            else if (Input.GetKey(KeyCode.I))
            {
                rotationInput = -1f;
            }
        }

        // Ber�kna rotationshastighet baserat p� knapptryckningar
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        //L�gger rotation p� vapnet
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Set the projectile's direction based on the player's facing direction
        Vector2 playerDirection = transform.right;
        projectile.GetComponent<Rigidbody2D>().velocity = playerDirection * projectile.GetComponent<BulletScript>().speed;
    }
}