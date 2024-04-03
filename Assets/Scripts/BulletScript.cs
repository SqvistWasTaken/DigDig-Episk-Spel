using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float damage;
    [SerializeField, Tooltip("The amount of time in seconds the target is unable to take action after hit")] private float stunDuration;
    [SerializeField] private bool isEnemyProjectile;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
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
                other.gameObject.GetComponent<playerhealth>().TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}