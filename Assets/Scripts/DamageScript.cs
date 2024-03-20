using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] private playerhealth script;
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (script != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                script.TakeDamage(0.25f);
                Debug.Log("Collision");
                Debug.Log(script.currentHealth);
                Debug.Log(script.currentHealth2);
            }
        }
    }
}
