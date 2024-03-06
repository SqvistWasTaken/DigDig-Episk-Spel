using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = startingHealth;
        Debug.Log(currentHealth);
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentHealth = currentHealth - 25;
        }
       
        Debug.Log("Collision");
        Debug.Log(currentHealth);
    }
}