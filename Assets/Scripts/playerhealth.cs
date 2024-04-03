using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [HideInInspector] public float health;

    [SerializeField] private AudioClip deathSound;
    private AudioSource source;

    private Animator anim;

    private void Start()
    {
        health = maxHealth;
        anim = GetComponentInChildren<Animator>();
        source = GetComponentInChildren<AudioSource>();
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

            source.pitch = Random.Range(0.8f, 1.2f);
            source.PlayOneShot(deathSound);
            
            Destroy(gameObject, 2);
        }
    }
}
