using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [HideInInspector] public float health;

    private Animator anim;

    private void Start()
    {
        health = maxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(GetComponent<PlayerMovement>());
            Destroy(GetComponentInChildren<weaponScript>().gameObject);

            anim.SetBool("Dead", true);

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.CheckPlayers();
            }
            
            Destroy(gameObject, 2);
        }
    }
}
