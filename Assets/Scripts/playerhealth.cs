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
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = startingHealth;
        currentHealth2 = startingHealth2;
    }

    // Update is called once per frame
    public void TakeDamage(float Takedamage)
    {
        currentHealth = Mathf.Clamp(currentHealth - Takedamage, 0, startingHealth);

        if (currentHealth || currentHealth2 == 0)
        {
            //player dies (generally in a single hit and gets sent back to main menu scene.
            SceneManager.LoadScene("MainMenu");
        }
    }
}
