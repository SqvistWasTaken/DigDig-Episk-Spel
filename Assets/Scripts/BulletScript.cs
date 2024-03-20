using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Move the projectile in its forward direction
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D  other)
    {
        Debug.Log("wall hit");
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy hit");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}