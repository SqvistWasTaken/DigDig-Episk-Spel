using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    public GameObject Enemy;

    void Update()
    {
        // Move the projectile in its forward direction
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(Enemy);
        }
    }
}