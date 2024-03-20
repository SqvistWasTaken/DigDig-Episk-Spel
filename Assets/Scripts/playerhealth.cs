using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth;
    
    [SerializeField] public float startingHealth2;
    public float currentHealth2;

    private bool bothDead = false;

    [SerializeField] private bool player1 = true;

    [SerializeField] private playerhealth script1;
    [SerializeField] private playerhealth script2;

    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = startingHealth;
        currentHealth2 = startingHealth2;

        if (script1.currentHealth <= 0)
        {
            if (script2.currentHealth2 <= 0)
            {
                bothDead = true;
            }

            if (bothDead == true)
            {
                //player dies (generally in a single hit and gets sent back to main menu scene.
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    // Update is called once per frame
    public void TakeDamage(float Takedamage)
    {
        Takedamage = 0.25f;
        if (player1)
        {
            currentHealth = Mathf.Clamp(currentHealth - Takedamage, 0, startingHealth);
        }

        if (!player1)
        {
            currentHealth2 = Mathf.Clamp(currentHealth2 - Takedamage, 0, startingHealth2);
        }
    }
}
