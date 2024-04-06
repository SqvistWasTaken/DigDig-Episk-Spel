using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float damage;
    [SerializeField, Tooltip("The amount of time in seconds the target is unable to take action after hit")] private float stunDuration;
    [SerializeField, Tooltip("Prefabs instantiated on projectile destruction")] private GameObject[] destructionPrefabs;
    [SerializeField] private bool isEnemyProjectile;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isEnemyProjectile)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, stunDuration);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (isEnemyProjectile)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
        DestroyProjectile();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(!isEnemyProjectile)
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(damage, stunDuration);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (isEnemyProjectile)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        foreach(GameObject destructionPrefab in destructionPrefabs)
        {
            Instantiate(destructionPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}