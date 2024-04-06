using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon properties")]
    public GameObject projectilePrefab;
    [SerializeField, Tooltip("The time in seconds it takes to fire another projectile")] private float fireCooldown;
    [SerializeField] private float rotationSpeed, rotationAcceleration; // Justera för rotationshastighet
    [SerializeField] private bool isPlayer1 = true;

    private Transform player;
    private Collider2D playerCol;
    private SpriteRenderer sprite;
    private AudioSource source;
    [SerializeField] private AudioClip fireSound, helicopterSound;
    [SerializeField] private float minPitch, maxPitch;

    private bool canShoot = true; // Declare canShoot as a class-level variable
    private float rotationInput;

    private bool isHelicopter; // Easter egg, try rotating your gun for a while and you'll see...

    private void Start()
    {
        isHelicopter = false;
        player = GetComponentInParent<PlayerMovement>().transform;
        playerCol = GetComponentInParent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isHelicopter)
        {
            if (isPlayer1)
            {
                if(Input.GetButton("RotateL1") && Input.GetButton("RotateR1"))
                {
                    rotationInput = 0f;

                    if (canShoot)
                    {
                        Shoot();
                        canShoot = false; // Set canShoot to false to start cooldown
                        Invoke("ResetShootFlag", fireCooldown); // Invoke a method to reset the shoot flag after cooldown
                    }
                }
                else if (Input.GetButton("RotateL1")) // Kontrollera om knapp för högerrotation är nedtryckt
                {
                    rotationInput += rotationAcceleration * Time.deltaTime;
                }
                else if (Input.GetButton("RotateR1")) // Kontrollera om knapp för vänsterrotation är nedtryckt
                {
                    rotationInput -= rotationAcceleration * Time.deltaTime;
                }
                else
                {
                    rotationInput = 0f;
                }
            }

            if (!isPlayer1)
            {
                if (Input.GetButton("RotateL2") && Input.GetButton("RotateR2"))
                {
                    rotationInput = 0f;

                    if (canShoot)
                    {
                        Shoot();
                        canShoot = false; // Set canShoot to false to start cooldown
                        Invoke("ResetShootFlag", fireCooldown); // Invoke a method to reset the shoot flag after cooldown
                    }
                }
                else if (Input.GetButton("RotateL2"))
                {
                    rotationInput += rotationAcceleration * Time.deltaTime;
                }
                // Kontrollera om knapp för vänsterrotation är nedtryckt
                else if ((Input.GetButton("RotateR2")))
                {
                    rotationInput -= rotationAcceleration * Time.deltaTime;
                }
                else
                {
                    rotationInput = 0f;
                }
            }
        }
        else
        {
            playerCol.enabled = false;
            player.position = new Vector2(player.position.x, (player.position.y + Mathf.Abs(rotationInput) * 0.001f));
            if (!source.isPlaying)
            {
                source.pitch = 2f;
                source.PlayOneShot(helicopterSound);
            }
            if (player.position.y > 10f)
            {
                Destroy(player.gameObject);
            }
        }

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }

        // Beräkna rotationshastighet baserat på knapptryckningar
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        if(rotationInput > 100f || rotationInput < -100f) // Helicopter mode
        {
            isHelicopter = true;
        }

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

        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(fireSound);

        // Destroy the bullet after 2 seconds
        Destroy(projectile, 2f);
    }

    void ResetShootFlag()
    {
        canShoot = true;
    }
}
