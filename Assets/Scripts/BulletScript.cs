using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 5f;
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
            Debug.Log("enemy hit");
            if(!isEnemyProjectile)
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player hit");
            if (isEnemyProjectile)
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}